using System;
using System.Collections.Generic;
using MARN_API.Enums;

namespace MARN_API.Models
{
    public class ChatRoom
    {
        public long Id { get; set; }
        public ChatRoomType Type { get; set; }
        public long? RequestId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual BookingRequest? Request { get; set; }
        public virtual ICollection<ChatMessage> Messages { get; set; } = new HashSet<ChatMessage>();
        public virtual ICollection<ChatRoomParticipant> Participants { get; set; } = new HashSet<ChatRoomParticipant>();
    }
}



