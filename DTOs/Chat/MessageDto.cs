using System;

namespace MARN_API.DTOs.Chat
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public string SenderId { get; set; } = string.Empty;
        public string? SenderName { get; set; }
        public string ReceiverId { get; set; } = string.Empty;
        public string? ReceiverName { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
}
