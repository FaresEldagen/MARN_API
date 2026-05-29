using MARN_API.Data;
using MARN_API.DTOs.Assistant;
using MARN_API.Enums;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MARN_API.Services.Implementations
{
    public class AssistantChatService : IAssistantChatService
    {
        private const int MaxMessageLength = 8000;
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
        private readonly IAssistantChatRepo _assistantChatRepo;
        private readonly IAssistantAiClient _assistantAiClient;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<AssistantChatService> _logger;

        public AssistantChatService(
            IAssistantChatRepo assistantChatRepo,
            IAssistantAiClient assistantAiClient,
            AppDbContext dbContext,
            ILogger<AssistantChatService> logger)
        {
            _assistantChatRepo = assistantChatRepo;
            _assistantAiClient = assistantAiClient;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ServiceResult<SendAssistantMessageResponseDto>> SendMessageAsync(
            Guid userId,
            SendAssistantMessageRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var accessCheck = await ValidateAssistantAccessAsync(userId);
            if (!accessCheck.Success)
                return ServiceResult<SendAssistantMessageResponseDto>.Fail(accessCheck.Message!, resultType: accessCheck.ResultType);

            var content = request.Content.Trim();
            if (string.IsNullOrWhiteSpace(content))
                return ServiceResult<SendAssistantMessageResponseDto>.Fail("Message content cannot be empty.");

            if (content.Length > MaxMessageLength)
                return ServiceResult<SendAssistantMessageResponseDto>.Fail($"Message content cannot exceed {MaxMessageLength} characters.");

            var now = DateTime.UtcNow;
            AssistantSession session;

            if (request.SessionId.HasValue)
            {
                var existingSession = await _assistantChatRepo.GetSessionForUserAsync(request.SessionId.Value, userId);
                if (existingSession == null)
                    return ServiceResult<SendAssistantMessageResponseDto>.Fail("Assistant session not found.", resultType: ServiceResultType.NotFound);

                session = existingSession;
            }
            else
            {
                var sessionId = Guid.NewGuid();
                session = new AssistantSession
                {
                    SessionId = sessionId,
                    UserId = userId,
                    SessionName = sessionId.ToString(),
                    CreatedAt = now,
                    UpdatedAt = now
                };

                await _assistantChatRepo.AddSessionAsync(session);
            }

            session.UpdatedAt = now;
            await _assistantChatRepo.UpdateSessionAsync(session);

            var userMessage = new AssistantMessage
            {
                MessageId = Guid.NewGuid(),
                UserId = userId,
                SessionId = session.SessionId,
                Role = AssistantMessageRoles.User,
                ToolOnly = false,
                Content = content,
                CreatedAt = now
            };

            await _assistantChatRepo.AddMessageAsync(userMessage);

            AssistantAiResponse assistantResponse;
            try
            {
                assistantResponse = await _assistantAiClient.GetAssistantResponseAsync(session.SessionId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Assistant AI request failed for session {SessionId}", session.SessionId);
                return ServiceResult<SendAssistantMessageResponseDto>.Fail(
                    "Assistant response could not be generated. Your message was saved.",
                    resultType: ServiceResultType.BadRequest,
                    code: "ASSISTANT_AI_REQUEST_FAILED");
            }

            var assistantMessage = new AssistantMessage
            {
                MessageId = Guid.NewGuid(),
                UserId = userId,
                SessionId = session.SessionId,
                Role = AssistantMessageRoles.Assistant,
                ToolOnly = false,
                Content = assistantResponse.Content,
                ImagePathsJson = SerializeImagePaths(assistantResponse.ImagePaths),
                CreatedAt = DateTime.UtcNow
            };

            await _assistantChatRepo.AddMessageAsync(assistantMessage);

            session.UpdatedAt = assistantMessage.CreatedAt;
            await _assistantChatRepo.UpdateSessionAsync(session);

            return ServiceResult<SendAssistantMessageResponseDto>.Ok(
                new SendAssistantMessageResponseDto
                {
                    SessionId = session.SessionId,
                    AssistantMessage = MapMessage(assistantMessage)
                },
                code: "ZZ_ASSISTANT_MESSAGE_SENT_SUCCESSFULLY");
        }

        public async Task<ServiceResult<List<AssistantSessionDto>>> GetSessionsAsync(Guid userId)
        {
            var accessCheck = await ValidateAssistantAccessAsync(userId);
            if (!accessCheck.Success)
                return ServiceResult<List<AssistantSessionDto>>.Fail(accessCheck.Message!, resultType: accessCheck.ResultType);

            var sessions = await _assistantChatRepo.GetSessionsForUserAsync(userId);
            return ServiceResult<List<AssistantSessionDto>>.Ok(sessions);
        }

        public async Task<ServiceResult<List<AssistantMessageDto>>> GetMessagesAsync(Guid userId, Guid sessionId)
        {
            var accessCheck = await ValidateAssistantAccessAsync(userId);
            if (!accessCheck.Success)
                return ServiceResult<List<AssistantMessageDto>>.Fail(accessCheck.Message!, resultType: accessCheck.ResultType);

            var session = await _assistantChatRepo.GetSessionForUserAsync(sessionId, userId);
            if (session == null)
                return ServiceResult<List<AssistantMessageDto>>.Fail("Assistant session not found.", resultType: ServiceResultType.NotFound);

            var messages = await _assistantChatRepo.GetVisibleMessagesAsync(sessionId, userId);
            var result = messages
                .Where(m => m.Role is AssistantMessageRoles.User or AssistantMessageRoles.Assistant)
                .Select(MapMessage)
                .ToList();

            return ServiceResult<List<AssistantMessageDto>>.Ok(result);
        }

        public async Task<ServiceResult<AssistantSessionDto>> RenameSessionAsync(
            Guid userId,
            Guid sessionId,
            RenameAssistantSessionRequestDto request)
        {
            var accessCheck = await ValidateAssistantAccessAsync(userId);
            if (!accessCheck.Success)
                return ServiceResult<AssistantSessionDto>.Fail(accessCheck.Message!, resultType: accessCheck.ResultType);

            var sessionName = request.SessionName.Trim();
            if (string.IsNullOrWhiteSpace(sessionName))
                return ServiceResult<AssistantSessionDto>.Fail("Session name cannot be empty.");

            var session = await _assistantChatRepo.GetSessionForUserAsync(sessionId, userId);
            if (session == null)
                return ServiceResult<AssistantSessionDto>.Fail("Assistant session not found.", resultType: ServiceResultType.NotFound);

            session.SessionName = sessionName;
            session.UpdatedAt = DateTime.UtcNow;
            await _assistantChatRepo.UpdateSessionAsync(session);

            return ServiceResult<AssistantSessionDto>.Ok(MapSession(session), code: "ZZ_ASSISTANT_SESSION_RENAMED_SUCCESSFULLY");
        }

        private async Task<ServiceResult<bool>> ValidateAssistantAccessAsync(Guid userId)
        {
            var user = await _dbContext.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.DeletedAt != null)
                return ServiceResult<bool>.Fail("User not found.", resultType: ServiceResultType.Unauthorized);

            if (user.AccountStatus == Enums.Account.AccountStatus.Banned)
                return ServiceResult<bool>.Fail("Banned accounts cannot use assistant chat.", resultType: ServiceResultType.Forbidden);

            return ServiceResult<bool>.Ok(true);
        }

        private static AssistantMessageDto MapMessage(AssistantMessage message)
        {
            return new AssistantMessageDto
            {
                MessageId = message.MessageId,
                SessionId = message.SessionId,
                Role = message.Role,
                Content = message.Content,
                ImagePaths = DeserializeImagePaths(message.ImagePathsJson),
                CreatedAt = message.CreatedAt
            };
        }

        private static string? SerializeImagePaths(List<string> imagePaths)
        {
            return imagePaths.Count == 0
                ? null
                : JsonSerializer.Serialize(imagePaths, JsonOptions);
        }

        private static List<string> DeserializeImagePaths(string? imagePathsJson)
        {
            if (string.IsNullOrWhiteSpace(imagePathsJson))
                return [];

            try
            {
                return JsonSerializer.Deserialize<List<string>>(imagePathsJson, JsonOptions) ?? [];
            }
            catch (JsonException)
            {
                return [];
            }
        }

        private static AssistantSessionDto MapSession(AssistantSession session)
        {
            return new AssistantSessionDto
            {
                SessionId = session.SessionId,
                SessionName = session.SessionName,
                CreatedAt = session.CreatedAt,
                UpdatedAt = session.UpdatedAt
            };
        }
    }
}
