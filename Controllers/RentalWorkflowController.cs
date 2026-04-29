//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using MARN_API.DTOs.Common;
//using MARN_API.DTOs.Payments;
//using MARN_API.DTOs.RentalWorkflow;
//using MARN_API.Models;
//using MARN_API.Services.Interfaces;

//namespace MARN_API.Controllers
//{
//    [ApiController]
//    [Route("api/rentalworkflow")]
//    public class RentalWorkflowController : BaseController
//    {
//        private readonly IRentalWorkflowService _rentalWorkflowService;
//        private readonly IConfiguration _configuration;

//        public RentalWorkflowController(IRentalWorkflowService rentalWorkflowService, IConfiguration configuration)
//        {
//            _rentalWorkflowService = rentalWorkflowService;
//            _configuration = configuration;
//        }

//        /// <summary>
//        /// Returns all rental workflow records for testing and operational inspection.
//        /// </summary>
//        /// <response code="200">Returns a paged list of rental workflow records.</response>
//        [HttpGet]
//        [ProducesResponseType(typeof(PagedResult<RentalTransactionResponseDto>), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetAllTransactions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
//        {
//            var transactions = await _rentalWorkflowService.GetAllTransactionsAsync(pageNumber, pageSize);
//            return Ok(MapPagedResult(transactions));
//        }

//        /// <summary>
//        /// Returns rental workflow records for the currently authenticated user.
//        /// </summary>
//        /// <response code="200">Returns a paged list of the current user's rental workflow records.</response>
//        /// <response code="401">If the user is not authenticated.</response>
//        [Authorize]
//        [HttpGet("my")]
//        [ProducesResponseType(typeof(PagedResult<RentalTransactionResponseDto>), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        public async Task<IActionResult> GetMyTransactions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
//        {
//            if (!TryGetUserId(out var userId))
//            {
//                return Unauthorized("User ID not found in token");
//            }

//            var transactions = await _rentalWorkflowService.GetTransactionsByUserIdAsync(userId, pageNumber, pageSize);
//            return Ok(MapPagedResult(transactions));
//        }

//        /// <summary>
//        /// Returns rental workflow records associated with a specific property.
//        /// </summary>
//        /// <response code="200">Returns a paged list of rental workflow records for the property.</response>
//        [HttpGet("by-property/{propertyId:long}")]
//        [ProducesResponseType(typeof(PagedResult<RentalTransactionResponseDto>), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetTransactionsByPropertyId(long propertyId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
//        {
//            var transactions = await _rentalWorkflowService.GetTransactionsByPropertyIdAsync(propertyId, pageNumber, pageSize);
//            return Ok(MapPagedResult(transactions));
//        }

//        /// <summary>
//        /// Returns a single rental workflow record by its identifier.
//        /// </summary>
//        /// <response code="200">Returns the requested rental workflow record.</response>
//        /// <response code="404">If the rental workflow record is not found.</response>
//        [HttpGet("{id:guid}")]
//        [ProducesResponseType(typeof(RentalTransactionResponseDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> GetTransactionById(Guid id)
//        {
//            var transaction = await _rentalWorkflowService.GetTransactionByIdAsync(id);
//            if (transaction is null)
//            {
//                return NotFound(new ProblemDetails { Title = "Rental workflow record not found", Status = 404 });
//            }

//            return Ok(MapTransaction(transaction));
//        }

//        /// <summary>
//        /// Returns the rental workflow record linked to a payment record.
//        /// </summary>
//        /// <response code="200">Returns the rental workflow record linked to the payment.</response>
//        /// <response code="404">If the rental workflow record is not found.</response>
//        [HttpGet("by-payment/{paymentRecordId:long}")]
//        [ProducesResponseType(typeof(RentalTransactionResponseDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> GetTransactionByPaymentId(long paymentRecordId)
//        {
//            var transaction = await _rentalWorkflowService.GetTransactionByPaymentRecordIdAsync(paymentRecordId);
//            if (transaction is null)
//            {
//                return NotFound(new ProblemDetails { Title = "Rental workflow record not found", Status = 404 });
//            }

//            return Ok(MapTransaction(transaction));
//        }

//        /// <summary>
//        /// Returns the rental workflow record linked to a contract.
//        /// </summary>
//        /// <response code="200">Returns the rental workflow record linked to the contract.</response>
//        /// <response code="404">If the rental workflow record is not found.</response>
//        [HttpGet("by-contract/{contractId:long}")]
//        [ProducesResponseType(typeof(RentalTransactionResponseDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> GetTransactionByContractId(long contractId)
//        {
//            var transaction = await _rentalWorkflowService.GetTransactionByContractIdAsync(contractId);
//            if (transaction is null)
//            {
//                return NotFound(new ProblemDetails { Title = "Rental workflow record not found", Status = 404 });
//            }

//            return Ok(MapTransaction(transaction));
//        }

//        /// <summary>
//        /// Starts the rental checkout workflow for the authenticated renter using a property ID.
//        /// </summary>
//        /// <response code="200">Returns the hosted Stripe checkout URL.</response>
//        /// <response code="400">If the property or payout workflow preconditions are not met.</response>
//        /// <response code="401">If the user is not authenticated.</response>
//        [Authorize]
//        [HttpPost("checkout")]
//        [ProducesResponseType(typeof(UrlResponseDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDto request)
//        {
//            if (!TryGetUserId(out var userId))
//            {
//                return Unauthorized("User ID not found in token");
//            }

//            var domain = $"{Request.Scheme}://{Request.Host}";
//            var successUrl = domain + (_configuration["Stripe:CheckoutSuccessPath"] ?? "/api/payments/success");
//            var cancelUrl = domain + (_configuration["Stripe:CheckoutCancelPath"] ?? "/api/payments/cancel");

//            var checkoutUrl = await _rentalWorkflowService.StartCheckoutAsync(request.PropertyId, userId, successUrl, cancelUrl);
//            return Ok(new UrlResponseDto { Url = checkoutUrl });
//        }

//        private static PagedResult<RentalTransactionResponseDto> MapPagedResult(PagedResult<RentalTransaction> pagedResult)
//        {
//            return new PagedResult<RentalTransactionResponseDto>
//            {
//                Items = pagedResult.Items.Select(MapTransaction).ToList(),
//                PageNumber = pagedResult.PageNumber,
//                PageSize = pagedResult.PageSize,
//                TotalCount = pagedResult.TotalCount,
//                TotalPages = pagedResult.TotalPages
//            };
//        }

//        private static RentalTransactionResponseDto MapTransaction(RentalTransaction transaction)
//        {
//            return new RentalTransactionResponseDto
//            {
//                Id = transaction.Id,
//                RenterId = transaction.RenterId,
//                OwnerId = transaction.OwnerId,
//                PropertyId = transaction.PropertyId,
//                StripeSessionId = transaction.StripeSessionId,
//                PaymentId = transaction.PaymentId,
//                ContractId = transaction.ContractId,
//                Status = transaction.Status,
//                PaymentStatus = transaction.PaymentStatus,
//                CreatedAt = transaction.CreatedAt,
//                CompletedAt = transaction.CompletedAt
//            };
//        }
//    }
//}
