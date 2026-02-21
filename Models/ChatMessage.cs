using System;

namespace MARN_API.Models
{
    public class ChatMessage
    {
        public long Id { get; set; }
        public long ChatRoomId { get; set; }
        public Guid SenderId { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsEdited { get; set; } = false;
        public DateTime? EditedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ChatRoom ChatRoom { get; set; } = null!;
        public virtual ApplicationUser Sender { get; set; } = null!;
    }
}



