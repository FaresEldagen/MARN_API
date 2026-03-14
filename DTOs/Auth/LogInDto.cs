using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.Auth
{
    public class LogInDto
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(256, ErrorMessage = "Email address cannot exceed 256 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public bool RememberMe { get; set; } = false;
    }
}
