using MARN_API.Models;
using MARN_API.DTOs.Common;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MARN_API.DTOs.Property;

namespace MARN_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HomepageController : BaseController
    {
        private readonly IHomepageService _homepageService;
        private readonly ILogger<HomepageController> _logger;

        public HomepageController(IHomepageService homepageService, ILogger<HomepageController> logger)
        {
            _homepageService = homepageService;
            _logger = logger;
        }



        /// <summary>
        /// Retrieves recommended properties for the authenticated user.
        /// </summary>
        /// <returns>A list of recommended property cards.</returns>
        [HttpGet("recommendations")]
        [ProducesResponseType(typeof(ApiResponseDto<PropertySearchResultDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetRecommendations()
        {
            if (!TryGetUserId(out var userId))
                return UnauthorizedUserIdMissing();

            var result = await _homepageService.GetRecommendedPropertiesAsync(userId);
            return HandleServiceResult(result);
        }
    }
}
