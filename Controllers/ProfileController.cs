using MARN_API.DTOs;
using MARN_API.Enums;
using MARN_API.Models;
using MARN_API.Services.Implementations;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MARN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(
            IProfileService profileService,
            ILogger<ProfileController> logger)
        {
            _profileService = profileService;
            _logger = logger;
        }

        //public IActionResult Index()
        //{
        //    return Ok("Profile Controller is working!");
        //}

        //public IActionResult ChangePassword()
        //{
        //    return Ok("Change Password endpoint is working!");
        //}

        //public IActionResult UpdateProfile()
        //{
        //    return Ok("Update Profile endpoint is working!");
        //}

        //public IActionResult DeleteProfile()
        //{
        //    return Ok("Delete Profile endpoint is working!");
        //}

        /// <summary>
        /// Toggle Two-Factor Authentication (2FA) for the authenticated user.
        /// </summary>
        /// <param name="dto">Optional password for verification</param>
        /// <returns>Current 2FA status (enabled/disabled)</returns>
        /// <response code="200">Returns the current 2FA status after toggle</response>
        /// <response code="400">If toggle failed (invalid password, etc.)</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="429">If rate limit is exceeded</response>
        [Authorize]
        [HttpPost("toggle-2fa")]
        [EnableRateLimiting("StrictAuth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> ToggleTwoFactor(ToggleTwoFactorDto dto)
        {
            // remove this - just for debuging 
            // var debugUserPrinciple =User;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found in token");

            var result = await _profileService.ToggleTwoFactorAsync(userId, dto.Password);

            return result.Success
                ? Ok(new { message = result.Message, data = result.Data })
                : BadRequest(new { message = result.Message });
        }
    }
}
