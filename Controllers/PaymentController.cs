using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Stripe;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MARN_API.Enums;
using MARN_API.Models;
using Microsoft.AspNetCore.Http.HttpResults;

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
                return UnauthorizedUserIdMissing();

            var result = await _paymentService.CreatePaymentIntent(userId, paymentScheduleId);
            return HandleServiceResult(result);
        }


        /// <summary>
        /// Creates a Stripe Connect Express account for the owner (if not already created) and returns an onboarding link.
        /// </summary>
        /// <response code="200">Returns the Stripe onboarding URL for the owner's Connect account.</response>
        /// <response code="401">If the user is not authenticated, not found, or not an Owner.</response>
        /// <response code="400">If saving the Stripe account fails.</response>
        [Authorize(Roles = "Owner")]
        [HttpPost("connect-account")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateConnectAccount()
        {
            if (!TryGetUserId(out var userId))
                return UnauthorizedUserIdMissing();

            var result = await _paymentService.CreateOrGetConnectOnboardingLink(userId);
            return HandleServiceResult(result);
        }


        /// <summary>
        /// Withdraws all available (non-held) funds to the owner's connected Stripe account.
        /// </summary>
        /// <response code="200">Withdrawal initiated successfully.</response>
        /// <response code="401">If the user is not authenticated or not an Owner.</response>
        /// <response code="400">If no funds are available, Stripe account is not connected, or Stripe transfer fails.</response>
        [Authorize(Roles = "Owner")]
        [HttpPost("withdraw")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Withdraw()
        {
            if (!TryGetUserId(out var userId))
                return UnauthorizedUserIdMissing();

            var result = await _paymentService.Withdraw(userId);
            return HandleServiceResult(result);
        }



        /// <summary>
        /// [TEST ONLY] Checks the Stripe balance of the platform account and the owner's connected account.
        /// </summary>
        [HttpGet("check-balance")]
        public async Task<IActionResult> CheckBalance()
        {
            try
            {
                var balanceService = new BalanceService();
                var platformBalance = await balanceService.GetAsync();

                var result = ServiceResult<object>.Ok(new
                    {
                        Platform = platformBalance,
                    },
                    "Balances retrieved successfully."
                );
                return HandleServiceResult(result);

            }
            catch (StripeException e)
            {
                var result = ServiceResult<object>.Fail(e.Message);
                return HandleServiceResult(result);
            }
        }


        /// <summary>
        /// [TEST ONLY] Toops up the Stripe platform balance with 100000 USD (Available balance).
        /// Use this to test withdrawals if you have insufficient funds in test mode.
        /// </summary>
        [HttpPost("topup-test-balance")]
        public async Task<IActionResult> TopUpTestBalance()
        {
            _logger.LogInformation("Top-up Platform Balance for testing attempt.");

            try
            {
                var options = new ChargeCreateOptions
                {
                    Amount = 10000000, // $100,000 USD
                    Currency = "usd",
                    Source = "tok_bypassPending",
                    Description = "Test Top-up (USD) for Withdrawal testing",
                };
                var service = new ChargeService();
                await service.CreateAsync(options);

                _logger.LogInformation("Platform balance topped up successfully with 500,000 EGP (Available).");
                var result = ServiceResult<bool>.Ok(
                    true,
                    "Platform balance topped up successfully with 500,000 EGP (Available).",
                    ServiceResultType.Success,
                    code: "ZZ_TEST_PLATFORM_BALANCE_TOPPED_UP_SUCCESSFULLY");
                return HandleServiceResult(result);
            }
            catch (StripeException e)
            {
                _logger.LogError(e, "Stripe API Error while topping up platform balance: {Message}", e.StripeError?.Message);
                var message = e.StripeError?.Message ?? "Payment provider error.";
                if (e.StripeError?.Code == "balance_insufficient")
                {
                    message += " (Test Tip: Use the 'topup-test-balance' endpoint to add available funds to your Stripe platform account for testing).";
                }

                var result = ServiceResult<bool>.Fail(message, resultType: ServiceResultType.BadRequest);
                return HandleServiceResult(result);
            }
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
            _logger.LogInformation("Incoming Stripe Webhook request received.");
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            Event stripeEvent;
            try
            {
                // Try validating with the standard webhook secret first
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _configuration["Stripe:WebhookSecret"]
                );
            }
            catch (StripeException)
            {
                try
                {
                    // If that fails, try the Connect webhook secret
                    stripeEvent = EventUtility.ConstructEvent(
                        json,
                        Request.Headers["Stripe-Signature"],
                        _configuration["Stripe:ConnectWebhookSecret"]
                    );
                }
                catch (StripeException e)
                {
                    _logger.LogError(e, "Stripe webhook signature validation failed for both standard and Connect secrets: {Message}", e.StripeError?.Message);
                    return BadRequestWebhookSignatureInvalid();
                }
            }
            
            _logger.LogInformation("Stripe Webhook signature validated. Event Type: {EventType}", stripeEvent.Type);

            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var intent = stripeEvent.Data.Object as PaymentIntent;
                await _paymentService.HandleSuccessfulPayment(intent!);
            }
            else if (stripeEvent.Type == "payment_intent.payment_failed")
            {
                var intent = stripeEvent.Data.Object as PaymentIntent;
                await _paymentService.HandleFailedPayment(intent!);
            }
            else if (stripeEvent.Type == "payment_intent.processing")
            {
                _logger.LogInformation("Payment is processing...");
            }

            else if (stripeEvent.Type == "account.updated")
            {
                var account = stripeEvent.Data.Object as Account;
                await _paymentService.HandleConnectedAccountUpdated(account!);
            }

            else if (stripeEvent.Type == "transfer.created")
            {
                var transfer = stripeEvent.Data.Object as Transfer;
                await _paymentService.HandleTransferCreated(transfer!);
            }
            //else if (stripeEvent.Type == "transfer.reversed")
            //{
            //    var transfer = stripeEvent.Data.Object as Transfer;
            //    await _paymentService.HandleTransferReversed(transfer!);
            //}

            return Ok();
        }
        #endregion
    }
}
