using MARN_API.DTOs.Dashboard;
using MARN_API.DTOs.Profile;
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
    public class ProfileController : BaseController
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

        #region Profile and Dashboards
        /// <summary>
        /// Return the renter dashboard data for this user for the authenticated user.
        /// </summary>
        /// <returns>
        /// Renter dashboard data for this user
        /// - Active rentals count
        /// - Next payment info (amount, due date, is paid) for the next pending payment across all active rentals
        /// - Saved properties count
        /// - Unread renter notifications count
        /// - Account status (verified, suspended, etc.)
        /// - Collections of active rentals (Active rental card contains contract id, contract status, start date, end date, property title, address, primary image url, rental period, next payment amount, due date, is paid (if there isn't these three will return null)) if there is any active rentals.
        /// - Collections of pending booking requests (Booking request card contains request id, request status, start date, end date,property Id, property title, owner Id, owner name, owner profile image) if there is any pending booking requests.
        /// - Collections of saved properties (property card contains property id, title, address, primary image url, price, rental unit, type, average rating, ratings count, max occupants, bedrooms count, bathrooms count) if there is any saved properties.
        /// - Collections of notifications (notification card contains notification id, title, is read, created at) if there is any notifications.
        /// - Collections of personalized recommendations 
        /// </returns>
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

        /// <summary>
        /// Return the owner dashboard data for this user for the authenticated user.
        /// </summary>
        /// <returns>
        /// Owner dashboard data for this user
        /// - Properties Count
        /// - Occupied Places Count
        /// - Vacant Places Count
        /// - Total Views Count
        /// - Monthly Earnings (month, year, amount)
        /// - Yearly Earnings (year, amount)
        /// - Withdrawable Earnings
        /// - On Hold Earnings
        /// - Unread owner notifications count
        /// - Pending booking requests count
        /// - Account status (verified, suspended, etc.)
        /// - Collections of owned properties (Property card contains property id, title, address, primary image url, price, rental unit, type, total views, average rating, ratings count, occupied places, total places, active contracts list which contains contract Id, renter Id, renter name, renter profile image) if there is any owned properties.
        /// - Collections of contracts (Contract card contains contract id, contract status, expiry date, renter id, renter name, property id, property title)
        /// - Collections of notifications (notification card contains notification id, title, is read, created at) if there is any notifications.
        /// - Collections of pending booking requests (Booking request card contains request id, request status, start date, end date,property Id, property title, renter Id, renter name, renter profile image) if there is any pending booking requests.
        /// </returns>
        /// <response code="200">Returns the Owner dashboard data for this user</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="429">If rate limit is exceeded</response>
        [Authorize("Owner")]
        [HttpGet("owner-dashboard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> OwnerDashboard()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized("User id not found in token");

            if (!Guid.TryParse(userIdString, out var userId))
                return Unauthorized("Invalid user id");

            var result = await _profileService.OwnerDashboardAsync(userId);
            return HandleServiceResult<OwnerDashboardDto>(result);
        }

        /// <summary>
        /// Return the personal profile data for the authenticated user.
        /// </summary>
        /// <returns>
        /// Profile data for this user
        /// - Basic Info ( Id, full name, email, profile image url, account status, date of birth, gender, country, member since, bio)
        /// - Owner Data (is owner, average rating, ratings count, owned properties count, collections of owned properties which contains property id, title, address, primary image url, price, rental unit, type, average rating, ratings count) if the user is owner.
        /// - Roommate Preferences (roommate preferences enabled, smoking, pets, sleep schedule, education level, field of study, noise tolerance, guests frequency, work schedule, sharing level, budget range min, budget range max) if the user is renter and roommate preferences enabled.
        /// </returns>
        /// <response code="200">Returns the personal profile data for this user</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="429">If rate limit is exceeded</response>
        [Authorize]
        [HttpGet("profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> GetPersonalProfile()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized("User id not found in token");

            if (!Guid.TryParse(userIdString, out var userId))
                return Unauthorized("Invalid user id");

            var result = await _profileService.GetProfileAsync(userId);
            return HandleServiceResult<ProfileDto>(result);
        }

        /// <summary>
        /// Return the personal profile data for the authenticated user.
        /// </summary>
        /// <param name="userId">Id for the person you want to view its profile</param>
        /// <returns>
        /// Profile data for this user
        /// - Basic Info ( Id, full name, email, profile image url, account status, date of birth, gender, country, member since, bio)
        /// - Owner Data (is owner, average rating, ratings count, owned properties count, collections of owned properties which contains property id, title, address, primary image url, price, rental unit, type, average rating, ratings count) if the user is owner.
        /// - Roommate Preferences (roommate preferences enabled, smoking, pets, sleep schedule, education level, field of study, noise tolerance, guests frequency, work schedule, sharing level, budget range min, budget range max) if the user is renter and roommate preferences enabled.
        /// </returns>
        /// <response code="200">Returns the personal profile data for this user</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="429">If rate limit is exceeded</response>
        [HttpGet("profile/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> GetProfile(Guid userId)
        {
            var result = await _profileService.GetProfileAsync(userId);
            return HandleServiceResult<ProfileDto>(result);
        }
        #endregion


        #region Profile Settings
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


        #endregion
    }
}
