using MARN_API.DTOs.Assistant;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IAssistantChatService
    {
        Task<ServiceResult<SendAssistantMessageResponseDto>> SendMessageAsync(Guid userId, SendAssistantMessageRequestDto request, CancellationToken cancellationToken = default);
        Task<ServiceResult<List<AssistantSessionDto>>> GetSessionsAsync(Guid userId);
        Task<ServiceResult<List<AssistantMessageDto>>> GetMessagesAsync(Guid userId, Guid sessionId);
        Task<ServiceResult<AssistantSessionDto>> RenameSessionAsync(Guid userId, Guid sessionId, RenameAssistantSessionRequestDto request);
    }
}
