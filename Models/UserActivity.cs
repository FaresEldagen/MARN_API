using System;
using MARN_API.Enums;

namespace MARN_API.Models
{
    public class UserActivity
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public UserActivityType Type { get; set; }
        public string? Description { get; set; }
        /// <summary>
        /// JSON metadata about the activity
        /// </summary>
        public string? Metadata { get; set; }
        public string? IPAddress { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ApplicationUser User { get; set; } = null!;
    }
}



