using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MARN_API.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }

        public bool RequiresTwoFactor { get; set; }
        public string? TwoFactorProvider { get; set; }

        public bool IsExternalLogin { get; set; }
        public string? ExternalProvider { get; set; } // e.g. "Google", "Facebook"
    }
}