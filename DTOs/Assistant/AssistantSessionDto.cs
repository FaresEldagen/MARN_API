namespace MARN_API.DTOs.Assistant
{
    public class AssistantSessionDto
    {
        public Guid SessionId { get; set; }
        public string SessionName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? LastMessagePreview { get; set; }
        public DateTime? LastMessageAt { get; set; }
    }
}
