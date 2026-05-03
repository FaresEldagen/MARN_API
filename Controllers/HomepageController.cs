using MARN_API.Models;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [AllowAnonymous]
        [HttpGet("recommendations")]
        [ProducesResponseType(typeof(List<MARN_API.DTOs.Property.PropertyCardDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecommendations()
        {
            Guid? userId = null;
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out Guid parsedId))
            {
                userId = parsedId;
            }

            var result = await _homepageService.GetRecommendedPropertiesAsync(userId);
            return HandleResult(result);
        }
    }
}
