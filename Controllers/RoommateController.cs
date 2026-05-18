using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MARN_API.Services.Interfaces;

namespace MARN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoommateController : BaseController
    {
        private readonly IRoommateMatchingService _matchingService;

        public RoommateController(IRoommateMatchingService matchingService)
        {
            _matchingService = matchingService;
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
                return UnauthorizedUserIdMissing();

            var result = await _matchingService.GetTopMatchesAsync(userId, limit);
            return HandleServiceResult<IEnumerable<MARN_API.DTOs.Roommate.RoommateMatchDto>>(result);
        }
    }
}
