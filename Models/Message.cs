using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MARN_API.Models
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(4000)]
        public string Content { get; set; } = string.Empty;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReadAt { get; set; }
        public bool IsHiddenByModeration { get; set; }
        public DateTime? HiddenAt { get; set; }
        public Guid? HiddenByAdminId { get; set; }
        public string? HiddenReason { get; set; }

        // Foreign Key for the Sender
        [Required]
        public Guid SenderId { get; set; } 

        [ForeignKey(nameof(SenderId))]
        public virtual ApplicationUser Sender { get; set; } = null!;

        // Foreign Key for the Receiver
        [Required]
        public Guid ReceiverId { get; set; } 

        [ForeignKey(nameof(ReceiverId))]
        public virtual ApplicationUser Receiver { get; set; } = null!;
    }
}
