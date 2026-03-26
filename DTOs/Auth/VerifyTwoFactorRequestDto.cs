using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MARN_API.DTOs.Auth
{
    public class VerifyTwoFactorDto
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Verification code is required.")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "Verification code must be between 6 and 10 characters.")]
        [RegularExpression(@"^\d{6,10}$", ErrorMessage = "Verification code must contain only digits.")]
        public string Code { get; set; } = string.Empty;

        [Required]
        public bool RememberMe { get; set; } = false;
    }
}