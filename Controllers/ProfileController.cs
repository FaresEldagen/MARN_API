using MARN_API.DTOs.Dashboard;
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


        protected ActionResult HandleServiceResult<T>(ServiceResult<T> result)
        {
            return result.ResultType switch
            {
                ServiceResultType.Success => Ok(new { message = result.Message, data = result.Data }),
                ServiceResultType.Created => StatusCode(201, new { message = result.Message, data = result.Data }),
                ServiceResultType.RequiresTwoFactor => Accepted(new { message = result.Message, data = result.Data }),
                ServiceResultType.Unauthorized => Unauthorized(new { message = result.Message }),
                ServiceResultType.NotFound => NotFound(new { message = result.Message }),
                ServiceResultType.Forbidden => StatusCode(403, new { message = result.Message }),
                ServiceResultType.Conflict => Conflict(new { message = result.Message, errors = result.Errors }),
                _ => BadRequest(new { message = result.Message, errors = result.Errors })
            };
        }



        /// <summary>
        /// Return the renter dashboard data for this user for the authenticated user.
        /// </summary>
        /// <returns>Renter dashboard data for this user</returns>
        /// <response code="200">Returns the renter dashboard data for this user</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="429">If rate limit is exceeded</response>
        [Authorize]
        [HttpGet("renter-dashboard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> RenterDashboard()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized("User id not found in token");

            if (!Guid.TryParse(userIdString, out var userId))
                return Unauthorized("Invalid user id");

            var result = await _profileService.RenterDashboardAsync(userId);
            return HandleServiceResult<RenterDashboardDto>(result);
        }

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found in token");

            var result = await _profileService.ToggleTwoFactorAsync(userId, dto.Password);
            return HandleServiceResult<bool>(result);
        }
    }
}
