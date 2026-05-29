using MARN_API.Attributes;
using MARN_API.DTOs.Assistant;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MARN_API.Controllers
{
    [Authorize]
    [DisallowBannedUser]
    [Route("api/[controller]")]
    [ApiController]
    public class AssistantController : BaseController
    {
        private readonly IAssistantChatService _assistantChatService;
        private readonly ILogger<AssistantController> _logger;

        public AssistantController(
            IAssistantChatService assistantChatService,
            ILogger<AssistantController> logger)
        {
            _assistantChatService = assistantChatService;
            _logger = logger;
        }

        /// <summary>
        /// Sends a user message to the assistant. Creates a new assistant session when sessionId is null.
        /// </summary>
        [HttpPost("messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SendMessage(
            [FromBody] SendAssistantMessageRequestDto request,
            CancellationToken cancellationToken)
        {
            if (!TryGetUserId(out var userId))
                return UnauthorizedUserIdMissing();

            _logger.LogInformation("User {UserId} sent an assistant message for session {SessionId}", userId, request.SessionId);

            var result = await _assistantChatService.SendMessageAsync(userId, request, cancellationToken);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Lists assistant chat sessions for the current user.
        /// </summary>
        [HttpGet("sessions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSessions()
        {
            if (!TryGetUserId(out var userId))
                return UnauthorizedUserIdMissing();

            var result = await _assistantChatService.GetSessionsAsync(userId);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Retrieves visible assistant chat history for a session.
        /// </summary>
        [HttpGet("sessions/{sessionId:guid}/messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMessages(Guid sessionId)
        {
            if (!TryGetUserId(out var userId))
                return UnauthorizedUserIdMissing();

            var result = await _assistantChatService.GetMessagesAsync(userId, sessionId);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Renames an assistant chat session.
        /// </summary>
        [HttpPatch("sessions/{sessionId:guid}/name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RenameSession(
            Guid sessionId,
            [FromBody] RenameAssistantSessionRequestDto request)
        {
            if (!TryGetUserId(out var userId))
                return UnauthorizedUserIdMissing();

            var result = await _assistantChatService.RenameSessionAsync(userId, sessionId, request);
            return HandleServiceResult(result);
        }
    }
}
