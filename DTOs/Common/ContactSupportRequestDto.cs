using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.Common
{
    public class ContactSupportRequestDto
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(150, ErrorMessage = "Full name cannot exceed 150 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(256, ErrorMessage = "Email address cannot exceed 256 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Phone number is not valid.")]
        [StringLength(30, ErrorMessage = "Phone number cannot exceed 30 characters.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject is required.")]
        [StringLength(200, ErrorMessage = "Subject cannot exceed 200 characters.")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Message is required.")]
        [StringLength(4000, ErrorMessage = "Message cannot exceed 4000 characters.")]
        public string Message { get; set; } = string.Empty;
    }
}
