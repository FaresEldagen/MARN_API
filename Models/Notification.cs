using MARN_API.Enums;
using System;

namespace MARN_API.Models
{
    public class Notification
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public NotificationType Type { get; set; }
        public NotificationUserType UserType { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        /// <summary>
        /// JSON payload with extra data
        /// </summary>
        public string? Data { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ApplicationUser User { get; set; } = null!;
    }
}



