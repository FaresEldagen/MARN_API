//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using MARN_API.DTOs.Common;
//using MARN_API.DTOs.Payments;
//using MARN_API.Models;
//using MARN_API.Services.Interfaces;
//using Stripe;

//namespace MARN_API.Controllers
//{
//    [ApiController]
//    [Route("api/payments")]
//    public class PaymentController : BaseController
//    {
//        private readonly IPaymentService _paymentService;
//        private readonly IRentalWorkflowService _rentalWorkflowService;
//        private readonly IConfiguration _configuration;
//        private readonly ILogger<PaymentController> _logger;

//        public PaymentController(IPaymentService paymentService, IRentalWorkflowService rentalWorkflowService, IConfiguration configuration, ILogger<PaymentController> logger)
//        {
//            _paymentService = paymentService;
//            _rentalWorkflowService = rentalWorkflowService;
//            _configuration = configuration;
//            _logger = logger;
//        }

//        /// <summary>
//        /// Returns all payment records for testing and admin-style inspection.
//        /// </summary>
//        /// <response code="200">Returns a paged list of payment records.</response>
//        [HttpGet]
//        [ProducesResponseType(typeof(PagedResult<PaymentResponseDto>), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetAllPayments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
//        {
//            var payments = await _paymentService.GetAllPaymentsAsync(pageNumber, pageSize);
//            return Ok(MapPagedResult(payments));
//        }

//        /// <summary>
//        /// Returns payment records that belong to the currently authenticated user.
//        /// </summary>
//        /// <response code="200">Returns a paged list of the current user's payment records.</response>
//        /// <response code="401">If the user is not authenticated.</response>
//        [Authorize]
//        [HttpGet("my")]
//        [ProducesResponseType(typeof(PagedResult<PaymentResponseDto>), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        public async Task<IActionResult> GetMyPayments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
//        {
//            if (!TryGetUserId(out var userId))
//            {
//                return Unauthorized("User ID not found in token");
//            }

//            var payments = await _paymentService.GetPaymentsByUserIdAsync(userId, pageNumber, pageSize);
//            return Ok(MapPagedResult(payments));
//        }

//        /// <summary>
//        /// Returns payment records associated with a specific property.
//        /// </summary>
//        /// <response code="200">Returns a paged list of payments for the specified property.</response>
//        [HttpGet("by-property/{propertyId:long}")]
//        [ProducesResponseType(typeof(PagedResult<PaymentResponseDto>), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetPaymentsByPropertyId(long propertyId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
//        {
//            var payments = await _paymentService.GetPaymentsByPropertyIdAsync(propertyId, pageNumber, pageSize);
//            return Ok(MapPagedResult(payments));
//        }

//        /// <summary>
//        /// Returns payment records associated with a specific user ID, whether as owner or renter.
//        /// </summary>
//        /// <response code="200">Returns a paged list of payments for the specified user.</response>
//        [HttpGet("by-user/{userId:guid}")]
//        [ProducesResponseType(typeof(PagedResult<PaymentResponseDto>), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetPaymentsByUserId(Guid userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
//        {
//            var payments = await _paymentService.GetPaymentsByUserIdAsync(userId, pageNumber, pageSize);
//            return Ok(MapPagedResult(payments));
//        }

//        /// <summary>
//        /// Returns a single payment record by its identifier.
//        /// </summary>
//        /// <response code="200">Returns the requested payment record.</response>
//        /// <response code="404">If the payment record is not found.</response>
//        [HttpGet("{paymentId:long}")]
//        [ProducesResponseType(typeof(PaymentResponseDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> GetPaymentById(long paymentId)
//        {
//            var payment = await _paymentService.GetPaymentByIdAsync(paymentId);
//            if (payment is null)
//            {
//                return NotFound(new ProblemDetails { Title = "Payment not found", Status = 404 });
//            }

//            return Ok(MapPayment(payment));
//        }

//        /// <summary>
//        /// Creates a Stripe checkout session for the authenticated renter using a property ID.
//        /// </summary>
//        /// <response code="200">Returns the hosted Stripe checkout URL.</response>
//        /// <response code="400">If the property or connected-account workflow preconditions are not met.</response>
//        /// <response code="401">If the user is not authenticated.</response>
//        [Authorize]
//        [HttpPost("checkout")]
//        [ProducesResponseType(typeof(UrlResponseDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        public async Task<IActionResult> CreateCheckoutSession([FromBody] CheckoutRequestDto request)
//        {
//            if (!TryGetUserId(out var userId))
//            {
//                return Unauthorized("User ID not found in token");
//            }

//            var domain = $"{Request.Scheme}://{Request.Host}";
//            var successUrl = domain + (_configuration["Stripe:CheckoutSuccessPath"] ?? "/api/payments/success");
//            var cancelUrl = domain + (_configuration["Stripe:CheckoutCancelPath"] ?? "/api/payments/cancel");

//            var url = await _rentalWorkflowService.StartCheckoutAsync(request.PropertyId, userId, successUrl, cancelUrl);
//            return Ok(new UrlResponseDto { Url = url });
//        }

//        /// <summary>
//        /// Receives Stripe webhook events for checkout completion and session expiration.
//        /// </summary>
//        /// <response code="200">If the webhook event was accepted and processed.</response>
//        /// <response code="400">If the Stripe signature or event payload is invalid.</response>
//        /// <response code="500">If the webhook event was valid but processing failed.</response>
//        [HttpPost("webhooks/stripe")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<IActionResult> Webhook()
//        {
//            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
//            var endpointSecret = _configuration["Stripe:WebhookSecret"];

//            try
//            {
//                var stripeSignature = Request.Headers["Stripe-Signature"].ToString();
//                var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, endpointSecret);

//                if (stripeEvent.Type == "checkout.session.completed" && stripeEvent.Data.Object is Stripe.Checkout.Session completedSession)
//                {
//                    await _paymentService.FulfillPaymentAsync(completedSession.Id);
//                }
//                else if (stripeEvent.Type == "checkout.session.expired" && stripeEvent.Data.Object is Stripe.Checkout.Session expiredSession)
//                {
//                    await _paymentService.ExpirePaymentAsync(expiredSession.Id);
//                }

//                return Ok();
//            }
//            catch (StripeException ex)
//            {
//                _logger.LogError(ex, "Stripe exception in webhook");
//                return BadRequest(new ProblemDetails
//                {
//                    Title = "Stripe webhook validation failed.",
//                    Detail = "The webhook signature or Stripe event payload could not be validated.",
//                    Status = StatusCodes.Status400BadRequest,
//                    Instance = HttpContext.Request.Path
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Exception in webhook");
//                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
//                {
//                    Title = "Webhook processing failed.",
//                    Detail = "The payment event was received but could not be fully processed.",
//                    Status = StatusCodes.Status500InternalServerError,
//                    Instance = HttpContext.Request.Path
//                });
//            }
//        }

//        /// <summary>
//        /// Temporary success callback endpoint used after Stripe checkout completes.
//        /// </summary>
//        /// <response code="200">Returns a success confirmation message.</response>
//        [HttpGet("success")]
//        [ProducesResponseType(typeof(MessageResponseDto), StatusCodes.Status200OK)]
//        public IActionResult Success()
//        {
//            return Ok(new MessageResponseDto { Message = "Payment successful!" });
//        }

//        /// <summary>
//        /// Temporary cancellation callback endpoint used when Stripe checkout is canceled.
//        /// </summary>
//        /// <response code="200">Returns a cancellation confirmation message.</response>
//        [HttpGet("cancel")]
//        [ProducesResponseType(typeof(MessageResponseDto), StatusCodes.Status200OK)]
//        public IActionResult Cancel()
//        {
//            return Ok(new MessageResponseDto { Message = "Payment was canceled." });
//        }

//        private static PagedResult<PaymentResponseDto> MapPagedResult(PagedResult<Payment> pagedResult)
//        {
//            return new PagedResult<PaymentResponseDto>
//            {
//                Items = pagedResult.Items.Select(MapPayment).ToList(),
//                PageNumber = pagedResult.PageNumber,
//                PageSize = pagedResult.PageSize,
//                TotalCount = pagedResult.TotalCount,
//                TotalPages = pagedResult.TotalPages
//            };
//        }

//        private static PaymentResponseDto MapPayment(Payment payment)
//        {
//            return new PaymentResponseDto
//            {
//                Id = payment.Id,
//                ContractId = payment.ContractId,
//                StripeSessionId = payment.StripeSessionId,
//                AmountTotal = payment.AmountTotal,
//                PlatformFee = payment.PlatformFee,
//                OwnerAmount = payment.OwnerAmount,
//                RenterId = payment.RenterId,
//                RenterEmail = payment.RenterEmail,
//                OwnerId = payment.OwnerId,
//                PropertyId = payment.PropertyId,
//                OwnerStripeAccountId = payment.OwnerStripeAccountId,
//                PaidAt = payment.PaidAt,
//                PaymentIntentId = payment.PaymentIntentId,
//                ReceiptUrl = payment.ReceiptUrl,
//                Currency = payment.Currency,
//                Status = payment.Status,
//                CreatedAt = payment.CreatedAt
//            };
//        }
//    }
//}
