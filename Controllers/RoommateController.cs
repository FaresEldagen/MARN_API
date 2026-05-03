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
    public class RoommateController : ControllerBase
    {
        private readonly IRoommateMatchingService _matchingService;
        private readonly IProfileService _profileService;

        public RoommateController(IRoommateMatchingService matchingService, IProfileService profileService)
        {
            _matchingService = matchingService;
            _profileService = profileService;
        }

        [HttpGet("matches")]
        public async Task<IActionResult> GetMatches([FromQuery] int limit = 10)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out Guid userId))
                return Unauthorized(new { Message = "Invalid user token." });

            var matches = await _matchingService.GetTopMatchesAsync(userId, limit);
            return Ok(matches);
        }

        [HttpPut("preferences")]
        public async Task<IActionResult> UpdatePreferences([FromBody] UpdateRoommatePreferencesDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out Guid userId) || userId != dto.UserId)
                return Unauthorized(new { Message = "Invalid user token or user ID mismatch." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _profileService.UpdateProfileRoommatePreferencesDataAsync(dto);
            if (!result.Success)
                return BadRequest(new { result.Message });

            return Ok(new { Message = "Roommate preferences updated successfully." });
        }
    }
}
