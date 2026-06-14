using System;

namespace MARN_API.Models
{
    public class PropertyFeedback
    {
        public long Id { get; set; }
        public long PropertyId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsHiddenByModeration { get; set; }
        public DateTime? HiddenAt { get; set; }
        public Guid? HiddenByAdminId { get; set; }
        public string? HiddenReason { get; set; }

        public virtual Property Property { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
