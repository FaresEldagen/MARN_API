using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MARN_API.DTOs.Common;
using MARN_API.DTOs.PropertyFeedback;
using MARN_API.Models;
using MARN_API.Services.Interfaces;

namespace MARN_API.Controllers
{
    /// <summary>
    /// Exposes property rating and comment endpoints.
    /// </summary>
    [ApiController]
    [Route("api/properties/{propertyId:long}")]
    public class PropertyFeedbackController : BaseController
    {
        private readonly IPropertyRatingService _propertyRatingService;
        private readonly IPropertyCommentService _propertyCommentService;

        public PropertyFeedbackController(
            IPropertyRatingService propertyRatingService,
            IPropertyCommentService propertyCommentService)
        {
            _propertyRatingService = propertyRatingService;
            _propertyCommentService = propertyCommentService;
        }

        /// <summary>
        /// Returns the rating summary for a property.
        /// </summary>
        /// <param name="propertyId">The property identifier.</param>
        /// <response code="200">Returns the average rating, ratings count, and the current user's rating when available.</response>
        /// <response code="404">If the property does not exist.</response>
        /// <response code="500">If an unexpected server error occurs.</response>
        [Authorize]
        [HttpGet("ratings/summary")]
        [ProducesResponseType(typeof(ApiResponseDto<PropertyRatingSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRatingSummary(long propertyId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(CreateErrorResponse(StatusCodes.Status401Unauthorized, "User ID not found in token"));

            var result = await _propertyRatingService.GetSummaryAsync(propertyId, userId);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Creates the current user's rating for a property, or updates it if one already exists.
        /// </summary>
        /// <param name="propertyId">The property identifier.</param>
        /// <param name="dto">The rating payload.</param>
        /// <response code="200">Returns the updated rating when the user has already rated this property.</response>
        /// <response code="201">Returns the newly created rating.</response>
        /// <response code="400">If the request payload is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have an eligible contract for this property.</response>
        /// <response code="404">If the property does not exist.</response>
        /// <response code="500">If an unexpected server error occurs.</response>
        [Authorize]
        [HttpPost("ratings")]
        [ProducesResponseType(typeof(ApiResponseDto<PropertyRatingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto<PropertyRatingDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRating(long propertyId, [FromBody] CreatePropertyRatingDto dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(CreateErrorResponse(StatusCodes.Status401Unauthorized, "User ID not found in token"));

            var result = await _propertyRatingService.CreateAsync(propertyId, userId, dto);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Updates the current user's rating for a property.
        /// </summary>
        /// <param name="propertyId">The property identifier.</param>
        /// <param name="dto">The updated rating payload.</param>
        /// <response code="200">Returns the updated rating.</response>
        /// <response code="400">If the request payload is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have an eligible contract for this property.</response>
        /// <response code="404">If the property or rating does not exist.</response>
        /// <response code="500">If an unexpected server error occurs.</response>
        [Authorize]
        [HttpPut("ratings/me")]
        [ProducesResponseType(typeof(ApiResponseDto<PropertyRatingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRating(long propertyId, [FromBody] UpdatePropertyRatingDto dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(CreateErrorResponse(StatusCodes.Status401Unauthorized, "User ID not found in token"));

            var result = await _propertyRatingService.UpdateAsync(propertyId, userId, dto);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Deletes the current user's rating for a property.
        /// </summary>
        /// <param name="propertyId">The property identifier.</param>
        /// <response code="200">Confirms that the rating was deleted successfully.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have an eligible contract for this property.</response>
        /// <response code="404">If the property or rating does not exist.</response>
        /// <response code="500">If an unexpected server error occurs.</response>
        [Authorize]
        [HttpDelete("ratings/me")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRating(long propertyId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(CreateErrorResponse(StatusCodes.Status401Unauthorized, "User ID not found in token"));

            var result = await _propertyRatingService.DeleteAsync(propertyId, userId);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Returns paged flat comments for a property ordered by newest first.
        /// </summary>
        /// <param name="propertyId">The property identifier.</param>
        /// <param name="pageNumber">The requested page number. Must be greater than zero.</param>
        /// <param name="pageSize">The requested page size. Must be greater than zero.</param>
        /// <response code="200">Returns a paged list of property comments.</response>
        /// <response code="400">If the paging parameters are invalid.</response>
        /// <response code="404">If the property does not exist.</response>
        /// <response code="500">If an unexpected server error occurs.</response>
        [Authorize]
        [HttpGet("comments")]
        [ProducesResponseType(typeof(ApiResponseDto<PagedResult<PropertyCommentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetComments(long propertyId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            if (!TryGetUserId(out _))
                return Unauthorized(CreateErrorResponse(StatusCodes.Status401Unauthorized, "User ID not found in token"));

            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest(CreateErrorResponse(StatusCodes.Status400BadRequest, "pageNumber and pageSize must be greater than 0."));
            }

            var result = await _propertyCommentService.GetByPropertyIdAsync(propertyId, pageNumber, pageSize);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Creates a new comment for the current user on a property.
        /// </summary>
        /// <param name="propertyId">The property identifier.</param>
        /// <param name="dto">The comment payload.</param>
        /// <response code="201">Returns the newly created comment.</response>
        /// <response code="400">If the request payload is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have an eligible contract for this property.</response>
        /// <response code="404">If the property does not exist.</response>
        /// <response code="500">If an unexpected server error occurs.</response>
        [Authorize]
        [HttpPost("comments")]
        [ProducesResponseType(typeof(ApiResponseDto<PropertyCommentMutationDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateComment(long propertyId, [FromBody] CreatePropertyCommentDto dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(CreateErrorResponse(StatusCodes.Status401Unauthorized, "User ID not found in token"));

            var result = await _propertyCommentService.CreateAsync(propertyId, userId, dto);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Updates an existing property comment owned by the current user.
        /// </summary>
        /// <param name="propertyId">The property identifier.</param>
        /// <param name="commentId">The comment identifier.</param>
        /// <param name="dto">The updated comment payload.</param>
        /// <response code="200">Returns the updated comment.</response>
        /// <response code="400">If the request payload is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not own the comment or has no eligible contract for this property.</response>
        /// <response code="404">If the property or comment does not exist.</response>
        /// <response code="500">If an unexpected server error occurs.</response>
        [Authorize]
        [HttpPut("comments/{commentId:long}")]
        [ProducesResponseType(typeof(ApiResponseDto<PropertyCommentMutationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateComment(long propertyId, long commentId, [FromBody] UpdatePropertyCommentDto dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(CreateErrorResponse(StatusCodes.Status401Unauthorized, "User ID not found in token"));

            var result = await _propertyCommentService.UpdateAsync(propertyId, commentId, userId, dto);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Deletes an existing property comment owned by the current user.
        /// </summary>
        /// <param name="propertyId">The property identifier.</param>
        /// <param name="commentId">The comment identifier.</param>
        /// <response code="200">Confirms that the comment was deleted successfully.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not own the comment or has no eligible contract for this property.</response>
        /// <response code="404">If the property or comment does not exist.</response>
        /// <response code="500">If an unexpected server error occurs.</response>
        [Authorize]
        [HttpDelete("comments/{commentId:long}")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteComment(long propertyId, long commentId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(CreateErrorResponse(StatusCodes.Status401Unauthorized, "User ID not found in token"));

            var result = await _propertyCommentService.DeleteAsync(propertyId, commentId, userId);
            return HandleServiceResult(result);
        }
    }
}
