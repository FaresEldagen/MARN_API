using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MARN_API.DTOs.Common;
using MARN_API.DTOs.PropertyFeedback;
using MARN_API.Models;
using MARN_API.Services.Interfaces;

namespace MARN_API.Controllers
{
    /// <summary>
    /// Exposes unified property feedback endpoints.
    /// </summary>
    [ApiController]
    [Route("api/properties/{propertyId:long}/feedback")]
    public class PropertyFeedbackController : BaseController
    {
        private readonly IPropertyFeedbackService _propertyFeedbackService;

        public PropertyFeedbackController(IPropertyFeedbackService propertyFeedbackService)
        {
            _propertyFeedbackService = propertyFeedbackService;
        }

        /// <summary>
        /// Returns rating summary and paged visible feedback for a property.
        /// </summary>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseDto<PropertyFeedbackSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFeedback(long propertyId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            if (!TryGetUserId(out var userId))
                return UnauthorizedUserIdMissing();

            if (pageNumber < 1 || pageSize < 1)
                return BadRequestInvalidPagination();

            var result = await _propertyFeedbackService.GetAsync(propertyId, userId, pageNumber, pageSize);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Creates the current user's one feedback row for a property.
        /// </summary>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseDto<PropertyFeedbackDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateFeedback(long propertyId, [FromBody] PropertyFeedbackRequestDto dto)
        {
            if (!TryGetUserId(out var userId))
                return UnauthorizedUserIdMissing();

            var result = await _propertyFeedbackService.CreateAsync(propertyId, userId, dto);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Updates the current user's feedback for a property.
        /// </summary>
        [Authorize]
        [HttpPut("me")]
        [ProducesResponseType(typeof(ApiResponseDto<PropertyFeedbackDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateFeedback(long propertyId, [FromBody] PropertyFeedbackRequestDto dto)
        {
            if (!TryGetUserId(out var userId))
                return UnauthorizedUserIdMissing();

            var result = await _propertyFeedbackService.UpdateAsync(propertyId, userId, dto);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Deletes the current user's feedback for a property.
        /// </summary>
        [Authorize]
        [HttpDelete("me")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFeedback(long propertyId)
        {
            if (!TryGetUserId(out var userId))
                return UnauthorizedUserIdMissing();

            var result = await _propertyFeedbackService.DeleteAsync(propertyId, userId);
            return HandleServiceResult(result);
        }
    }
}
