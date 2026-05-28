using MARN_API.DTOs.Assistant;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IAssistantChatRepo
    {
        Task<AssistantSession?> GetSessionForUserAsync(Guid sessionId, Guid userId);
        Task<List<AssistantSessionDto>> GetSessionsForUserAsync(Guid userId);
        Task<List<AssistantMessage>> GetVisibleMessagesAsync(Guid sessionId, Guid userId);
        Task<AssistantSession> AddSessionAsync(AssistantSession session);
        Task<AssistantMessage> AddMessageAsync(AssistantMessage message);
        Task UpdateSessionAsync(AssistantSession session);
    }
}
