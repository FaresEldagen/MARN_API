using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MARN_API.Models
{
    public class AssistantMessage
    {
        [Key]
        public Guid MessageId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        public Guid SessionId { get; set; }

        [ForeignKey(nameof(SessionId))]
        public virtual AssistantSession Session { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = string.Empty;

        public bool ToolOnly { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public string? ImagePathsJson { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
