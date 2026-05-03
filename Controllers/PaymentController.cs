using MARN_API.DTOs.BookingRequest;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MARN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        /// <summary>
        /// </summary>
        /// <param name="paymentScheduleId"></param>
        /// <response code="200"></response>
        /// <response code="401"></response>
        /// <response code="404"></response>
        [HttpPost("start-payment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> StartPayment(long paymentScheduleId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            var result = await _paymentService.CreatePaymentIntent(userId, paymentScheduleId);
            return HandleServiceResult(result);
        }


    }
}
