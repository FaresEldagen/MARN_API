using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MARN_API.DTOs
{
    public class ToggleTwoFactorDto
    {
        public string? Password { get; set; } // optional but recommended
    }
}