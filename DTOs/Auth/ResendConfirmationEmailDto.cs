using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.Auth
{
    public class ResendConfirmationEmailDto
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;
    }
}
