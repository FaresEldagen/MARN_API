namespace MARN_API.DTOs.Assistant
{
    public class SendAssistantMessageResponseDto
    {
        public Guid SessionId { get; set; }
        public AssistantMessageDto AssistantMessage { get; set; } = null!;
    }
}
