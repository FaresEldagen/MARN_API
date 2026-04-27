using System;

namespace MARN_API.Models
{
    public class PropertyComment
    {
        public long Id { get; set; }
        public long PropertyId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public virtual Property Property { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
