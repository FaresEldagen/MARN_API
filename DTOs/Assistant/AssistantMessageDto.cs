namespace MARN_API.DTOs.Assistant
{
    public class AssistantMessageDto
    {
        public Guid MessageId { get; set; }
        public Guid SessionId { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<string> ImagePaths { get; set; } = [];
        public DateTime CreatedAt { get; set; }
    }
}
