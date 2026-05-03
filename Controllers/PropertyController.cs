using MARN_API.DTOs.Property;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using MARN_API.DTOs.Dashboard;
using MARN_API.Models;
using Microsoft.AspNetCore.Identity;

namespace MARN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : BaseController
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }



        /// <summary>
        /// Adds the Owner role to the authenticated user and returns a new JWT token.
        /// </summary>
        /// <response code="200">Owner role added successfully and new JWT returned.</response>
        /// <response code="400">If validation fails or user not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [Authorize]
        [HttpPost("become-owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> BecomeAnOwner([FromServices] IOwnerService ownerService)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            var result = await ownerService.AddOwnerRole(userId);
            return HandleServiceResult<string>(result);
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
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            var result = await _propertyService.AddPropertyAsync(dto, userId);
            return HandleServiceResult<bool>(result);
        }


        /// <summary>
        /// Searches and filters property listings with sorting and pagination.
        /// Anonymous users can use this endpoint. Authenticated users get saved-state on each card.
        /// </summary>
        /// <param name="filter">
        /// All filter, sort, and pagination parameters are optional query-string fields:
        /// - Keyword, Latitude, Longitude, RadiusKm, City (enum), Governorate (enum)
        /// - Type, RentalUnit, IsShared
        /// - MinPrice, MaxPrice
        /// - MinBedrooms, MinBeds, MinBathrooms, MinMaxOccupants
        /// - MinSquareMeters, MaxSquareMeters
        /// - MinRating
        /// - Amenities (list)
        /// - SortBy (enum: Newest | Price | Rating | Bedrooms | Bathrooms | SquareMeters | Distance)
        /// - SortAscending
        /// - Page, PageSize
        /// </param>
        /// <response code="200">Paginated list of property cards matching the filters</response>
        /// <response code="429">If rate limit is exceeded</response>
        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> SearchProperties([FromQuery] PropertySearchFilterDto filter)
        {
            Guid? userId = null;
            if (TryGetUserId(out var parsedUserId))
            {
                userId = parsedUserId;
            }

            var result = await _propertyService.SearchPropertiesAsync(filter, userId);
            return HandleServiceResult<PropertySearchResultDto>(result);
        }


        /// <summary>
        /// Retrieves full property details with user-context and owner-only extras.
        /// </summary>
        /// <param name="propertyId">ID of the property.</param>
        /// <response code="200">Property details returned successfully.</response>
        /// <response code="404">Property not found.</response>
        /// <response code="429">If rate limit is exceeded</response>
        [AllowAnonymous]
        [HttpGet("{propertyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> GetProperty(long propertyId)
        {
            Guid? userId = null;
            if (TryGetUserId(out var parsedUserId))
            {
                userId = parsedUserId;
            }

            var result = await _propertyService.GetPropertyDetailsAsync(propertyId, userId);
            return HandleServiceResult<PropertyDetailsDto>(result);
        }


        /// <summary>
        /// Retrieves the editable data details for a specific property including existing images and rule sets.
        /// </summary>
        /// <param name="propertyId">ID string of the property being retrieved</param>
        /// <response code="200">Retrieval succeeded</response>
        /// <response code="401">Unauthorized caller</response>
        /// <response code="403">Caller doesn't own this property</response>
        /// <response code="404">Property not found</response>
        /// <response code="429">If rate limit is exceeded</response>
        [Authorize(Roles = "Owner")]
        [HttpGet("edit/{propertyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> GetPropertyEdit(long propertyId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            var result = await _propertyService.GetPropertyEditAsync(propertyId, userId);
            return HandleServiceResult<PropertyEditDataDto>(result);
        }


        /// <summary>
        /// Submits an edit layout for modifying an existing property listing structure.
        /// </summary>
        /// <param name="propertyId">ID strings for updating matching property.</param>
        /// <param name="dto">The updated contents of the property.</param>
        /// <response code="200">Edits persisted seamlessly</response>
        /// <response code="401">Unauthorized requester</response>
        /// <response code="403">Requester fails ownership verification</response>
        /// <response code="429">If rate limit is exceeded</response>
        [Authorize(Roles = "Owner")]
        [HttpPut("edit/{propertyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> EditProperty(long propertyId, [FromForm] EditPropertyDto dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            var result = await _propertyService.EditPropertyAsync(propertyId, dto, userId);
            return HandleServiceResult<bool>(result);
        }


        /// <summary>
        /// Toggles a property's active status dynamically rendering it unsearchable or searchable.
        /// </summary>
        /// <param name="propertyId">ID strings for matching property.</param>
        /// <response code="200">Reactivation/Deactivation persisted seamlessly</response>
        /// <response code="401">Unauthorized requester</response>
        /// <response code="403">Requester fails ownership verification</response>
        /// <response code="429">If rate limit is exceeded</response>
        [Authorize(Roles = "Owner")]
        [HttpPut("deactivate/{propertyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> DeactivateProperty(long propertyId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            var result = await _propertyService.DeactivatePropertyAsync(propertyId, userId);
            return HandleServiceResult<bool>(result);
        }


        /// <summary>
        /// Saves or unsaves a property for the authenticated user.
        /// </summary>
        /// <param name="propertyId">ID string of the property being saved/unsaved.</param>
        /// <response code="200">Save state toggled successfully</response>
        /// <response code="401">Unauthorized requester</response>
        /// <response code="404">Property not found</response>
        /// <response code="429">If rate limit is exceeded</response>
        [Authorize]
        [HttpPost("save/{propertyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> SaveProperty(long propertyId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            var result = await _propertyService.ToggleSavePropertyAsync(propertyId, userId);
            return HandleServiceResult<bool>(result);
        }


        /// <summary>
        /// Triggers a nested deletion operation hard deleting connected bookings/images entirely and restricting access to older contracts smoothly.
        /// </summary>
        /// <param name="propertyId">ID strings for updating matching property.</param>
        /// <response code="200">Deletions effectively propagated securely</response>
        /// <response code="400">Halts immediately if the property is blocked by an active rent contract.</response>
        /// <response code="401">Unauthorized requester</response>
        /// <response code="403">Requester fails ownership verification</response>
        /// <response code="429">If rate limit is exceeded</response>
        [Authorize(Roles = "Owner")]
        [HttpDelete("delete/{propertyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> DeleteProperty(long propertyId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            var result = await _propertyService.DeletePropertyAsync(propertyId, userId);
            return HandleServiceResult<bool>(result);
        }
    }
}
