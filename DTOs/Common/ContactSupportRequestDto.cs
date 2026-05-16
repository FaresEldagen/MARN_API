using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.Common
{
    public class ContactSupportRequestDto
    {
        [Required]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(30)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [StringLength(4000)]
        public string Message { get; set; } = string.Empty;
    }
}
