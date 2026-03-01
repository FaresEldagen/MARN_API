using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MARN_API.DTOs
{
    public class ToggleTwoFactorDto
    {
        [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
        public string? Password { get; set; } // optional but recommended
    }
}