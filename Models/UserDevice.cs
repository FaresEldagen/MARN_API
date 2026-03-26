using System;
using System.ComponentModel.DataAnnotations;

namespace MARN_API.Models
{
    public class UserDevice
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public string FcmToken { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }
    }
}
