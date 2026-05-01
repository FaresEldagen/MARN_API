using MARN_API.DTOs.BookingRequest;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MARN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingRequestController : BaseController
    {
        private readonly IBookingRequestService _bookingRequestService;

        public BookingRequestController(IBookingRequestService bookingRequestService)
        {
            _bookingRequestService = bookingRequestService;
        }

        /// <summary>
        /// Adds a new booking request for a specific property.
        /// </summary>
        /// <param name="dto">
        /// The booking request data:
        /// - PropertyId: The ID of the property to book.
        /// - StartDate: The start date of the booking.
        /// - EndDate: The end date of the booking.
        /// - PaymentFrequency: The preferred payment frequency for the booking.
        /// </param>
        /// <response code="200">Booking request added successfully.</response>
        /// <response code="400">If validation fails or duration is not divisible by the property's rental unit.</response>
        /// <response code="401">If the user is not authenticated or the user account is not verified.</response>
        /// <response code="404">If the property does not exist or is not active.</response>
        /// <response code="409">If the property already has active contracts.</response>
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddBookingRequest([FromBody] AddBookingRequestDto dto)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized(CreateErrorResponse(StatusCodes.Status401Unauthorized, "Invalid user ID format."));
            }

            var result = await _bookingRequestService.AddBookingRequestAsync(userId, dto);
            return HandleServiceResult(result);
        }
    }
}
