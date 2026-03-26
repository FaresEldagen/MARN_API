using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.Profile
{
    public class ChangePasswordDto
    {
        [Required]
        public Guid id { get; set; }

        [Required(ErrorMessage = "Current Password is Required")]
        public string CurrentPassword { get; set; } = null!;

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "Confirming The Password is Required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; } = null!;
    }
}
