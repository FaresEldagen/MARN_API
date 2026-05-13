using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MARN_API.Services.Interfaces;
using MARN_API.DTOs.Profile;

namespace MARN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoommateController : BaseController
    {
        private readonly IRoommateMatchingService _matchingService;
        private readonly IProfileService _profileService;

        public RoommateController(
            IRoommateMatchingService matchingService, 
            IProfileService profileService)
        {
            _matchingService = matchingService;
            _profileService = profileService;
        }

        /// <summary>
        /// Retrieves the top roommate matches for the authenticated user based on their preferences.
        /// </summary>
        /// <param name="limit">The maximum number of matches to return. Defaults to 10.</param>
        /// <response code="200">Returns list of compatibility-ranked roommate profiles.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet("matches")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMatches([FromQuery] int limit = 10)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            var result = await _matchingService.GetTopMatchesAsync(userId, limit);
            return HandleServiceResult<IEnumerable<MARN_API.DTOs.Roommate.RoommateMatchDto>>(result);
        }

        /// <summary>
        /// Updates the roommate matching preferences for the authenticated user.
        /// </summary>
        /// <param name="dto">The updated roommate preferences.</param>
        /// <response code="200">Returns success message if preferences are updated.</response>
        /// <response code="400">If validation fails or user not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpPut("preferences")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdatePreferences([FromBody] UpdateRoommatePreferencesDto dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            if (userId != dto.UserId)
                return Unauthorized("User ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _profileService.UpdateProfileRoommatePreferencesDataAsync(dto);
            return HandleServiceResult<bool>(result);
        }
    }
}
