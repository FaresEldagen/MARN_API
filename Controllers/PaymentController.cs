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


        /// <summary>
        /// </summary>
        [AllowAnonymous]
        [HttpPost("webhook")]
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
    }
}
