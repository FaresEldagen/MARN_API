using MARN_API.DTOs.Property;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace MARN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }


        /// <summary>
        /// Add a new property listings for the authenticated user.
        /// </summary>
        /// <param name="dto">
        /// Property details to create:
        /// - Title, Description, Type, IsShared, MaxOccupants, Bedrooms, Beds, Bathrooms, Price, RentalUnit, Address, City, State, ZipCode, SquareMeters, ProofOfOwnership, Latitude, Longitude, Amenities, Rules, PrimaryImage, MediaFiles
        /// </param>
        /// <response code="200">Property added successfully</response>
        /// <response code="400">If validation fails, user not found, or account is not verified</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="429">If rate limit is exceeded</response>
        [Authorize]
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> AddProperty([FromForm] AddPropertyDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized();
            }

            var result = await _propertyService.AddPropertyAsync(dto, userId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }
    }
}
