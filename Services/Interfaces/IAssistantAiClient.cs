namespace MARN_API.Services.Interfaces
{
    public interface IAssistantAiClient
    {
        Task<string> GetAssistantResponseAsync(Guid sessionId, CancellationToken cancellationToken = default);
    }
}
