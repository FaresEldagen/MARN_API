using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Stripe;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MARN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IPaymentService paymentService, 
            IConfiguration configuration,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _configuration = configuration;
            _logger = logger;
        }


        #region Payment
        /// <summary>
        /// Create a Stripe Payment Intent for a specific payment schedule.
        /// </summary>
        /// <param name="paymentScheduleId">The ID of the payment schedule to pay for.</param>
        /// <response code="200">
        /// Returns the Stripe client secret required to complete the payment on the frontend.
        /// </response>
        /// <response code="401">If the user is not authenticated or user ID is missing from token.</response>
        /// <response code="404">If the payment schedule is not found or does not belong to the user.</response>
        /// <response code="429">If rate limit is exceeded.</response>
        [HttpPost("start-payment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> StartPayment(long paymentScheduleId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            var result = await _paymentService.CreatePaymentIntent(userId, paymentScheduleId);
            return HandleServiceResult(result);
        }


        /// <summary>
        /// Stripe Webhook endpoint to handle payment events (success, failure, processing). [No thing to deal with as a frontend or flutter]
        /// </summary>
        /// <response code="200">Webhook processed successfully.</response>
        /// <response code="400">If the Stripe signature validation fails.</response>
        [AllowAnonymous]
        [HttpPost("webhook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _configuration["Stripe:WebhookSecret"]
                );

                var intent = stripeEvent.Data.Object as PaymentIntent;

                if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    await _paymentService.HandleSuccessfulPayment(intent!);
                }
                else if (stripeEvent.Type == "payment_intent.payment_failed")
                {
                    await _paymentService.HandleFailedPayment(intent!);
                }
                else if (stripeEvent.Type == "payment_intent.processing")
                {
                    _logger.LogInformation("Payment is processing...");
                }
            }
            catch (StripeException e)
            {
                _logger.LogError(e, "Stripe webhook signature validation failed: {Message}", e.StripeError?.Message);
                return BadRequest();
            }

            return Ok();
        }
        #endregion
    }
}
