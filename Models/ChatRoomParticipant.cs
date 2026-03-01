using System;

namespace MARN_API.Models
{
    public class ChatRoomParticipant
    {
        public long ChatRoomId { get; set; }
        public Guid UserId { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastReadAt { get; set; }

        public virtual ChatRoom ChatRoom { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
    }
}



