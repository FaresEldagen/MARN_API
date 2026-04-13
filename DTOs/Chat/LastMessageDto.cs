namespace MARN_API.DTOs.Chat
{
    public class LastMessageDto
    {
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public bool IsMine { get; set; }
    }
}
