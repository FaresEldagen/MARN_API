namespace MARN_API.Services.Interfaces
{
    using MARN_API.DTOs.Assistant;

    public interface IAssistantAiClient
    {
        Task<AssistantAiResponse> GetAssistantResponseAsync(Guid sessionId, CancellationToken cancellationToken = default);
    }
}
