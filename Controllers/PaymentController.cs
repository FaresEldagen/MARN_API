using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MARN_API.DTOs.Common;
using MARN_API.DTOs.Payments;
using MARN_API.Models;
using MARN_API.Services.Interfaces;
using Stripe;

namespace MARN_API.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IRentalWorkflowService _rentalWorkflowService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, IRentalWorkflowService rentalWorkflowService, IConfiguration configuration, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _rentalWorkflowService = rentalWorkflowService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var payments = await _paymentService.GetAllPaymentsAsync(pageNumber, pageSize);
            return Ok(MapPagedResult(payments));
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyPayments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized("User ID not found in token");
            }

            var payments = await _paymentService.GetPaymentsByUserIdAsync(userId, pageNumber, pageSize);
            return Ok(MapPagedResult(payments));
        }

        [HttpGet("by-property/{propertyId:long}")]
        public async Task<IActionResult> GetPaymentsByPropertyId(long propertyId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var payments = await _paymentService.GetPaymentsByPropertyIdAsync(propertyId, pageNumber, pageSize);
            return Ok(MapPagedResult(payments));
        }

        [Authorize]
        [HttpGet("{paymentId:long}")]
        public async Task<IActionResult> GetPaymentById(long paymentId)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized("User ID not found in token");
            }

            var payment = await _paymentService.GetPaymentByIdAsync(paymentId);
            if (payment is null)
            {
                return NotFound(new ProblemDetails { Title = "Payment not found", Status = 404 });
            }

            if (payment.OwnerId != userId && payment.RenterId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "You do not have access to this payment." });
            }

            return Ok(MapPayment(payment));
        }

        [Authorize]
        [HttpPost("checkout")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CheckoutRequestDto request)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized("User ID not found in token");
            }

            try
            {
                var domain = $"{Request.Scheme}://{Request.Host}";
                var successUrl = domain + (_configuration["Stripe:CheckoutSuccessPath"] ?? "/api/payments/success");
                var cancelUrl = domain + (_configuration["Stripe:CheckoutCancelPath"] ?? "/api/payments/cancel");

                var url = await _rentalWorkflowService.StartCheckoutAsync(request.PropertyId, userId, successUrl, cancelUrl);
                return Ok(new { url });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating checkout session");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("webhooks/stripe")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var endpointSecret = _configuration["Stripe:WebhookSecret"];

            try
            {
                var stripeSignature = Request.Headers["Stripe-Signature"].ToString();
                var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, endpointSecret);

                if (stripeEvent.Type == "checkout.session.completed" && stripeEvent.Data.Object is Stripe.Checkout.Session completedSession)
                {
                    await _paymentService.FulfillPaymentAsync(completedSession.Id);
                }
                else if (stripeEvent.Type == "checkout.session.expired" && stripeEvent.Data.Object is Stripe.Checkout.Session expiredSession)
                {
                    await _paymentService.ExpirePaymentAsync(expiredSession.Id);
                }

                return Ok();
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe exception in webhook");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in webhook");
                return StatusCode(500);
            }
        }

        [HttpGet("success")]
        public IActionResult Success()
        {
            return Ok("Payment successful!");
        }

        [HttpGet("cancel")]
        public IActionResult Cancel()
        {
            return Ok("Payment was canceled.");
        }

        private static PagedResult<PaymentResponseDto> MapPagedResult(PagedResult<Payment> pagedResult)
        {
            return new PagedResult<PaymentResponseDto>
            {
                Items = pagedResult.Items.Select(MapPayment).ToList(),
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount,
                TotalPages = pagedResult.TotalPages
            };
        }

        private static PaymentResponseDto MapPayment(Payment payment)
        {
            return new PaymentResponseDto
            {
                Id = payment.Id,
                ContractId = payment.ContractId,
                StripeSessionId = payment.StripeSessionId,
                AmountTotal = payment.AmountTotal,
                PlatformFee = payment.PlatformFee,
                OwnerAmount = payment.OwnerAmount,
                RenterId = payment.RenterId,
                RenterEmail = payment.RenterEmail,
                OwnerId = payment.OwnerId,
                PropertyId = payment.PropertyId,
                OwnerStripeAccountId = payment.OwnerStripeAccountId,
                DueDate = payment.DueDate,
                PaidAt = payment.PaidAt,
                AvailableAt = payment.AvailableAt,
                PaymentIntentId = payment.PaymentIntentId,
                ReceiptUrl = payment.ReceiptUrl,
                Currency = payment.Currency,
                Status = payment.Status,
                CreatedAt = payment.CreatedAt,
                PropertyTitle = payment.Property?.Title,
                PropertyAddress = payment.Property?.Address,
                RenterFirstName = payment.Renter?.FirstName,
                RenterLastName = payment.Renter?.LastName
            };
        }
    }
}
