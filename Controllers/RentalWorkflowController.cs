using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MARN_API.DTOs.Common;
using MARN_API.DTOs.Payments;
using MARN_API.DTOs.RentalWorkflow;
using MARN_API.Models;
using MARN_API.Services.Interfaces;

namespace MARN_API.Controllers
{
    [ApiController]
    [Route("api/rentalworkflow")]
    public class RentalWorkflowController : BaseController
    {
        private readonly IRentalWorkflowService _rentalWorkflowService;
        private readonly IConfiguration _configuration;

        public RentalWorkflowController(IRentalWorkflowService rentalWorkflowService, IConfiguration configuration)
        {
            _rentalWorkflowService = rentalWorkflowService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var transactions = await _rentalWorkflowService.GetAllTransactionsAsync(pageNumber, pageSize);
            return Ok(MapPagedResult(transactions));
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyTransactions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized("User ID not found in token");
            }

            var transactions = await _rentalWorkflowService.GetTransactionsByUserIdAsync(userId, pageNumber, pageSize);
            return Ok(MapPagedResult(transactions));
        }

        [HttpGet("by-property/{propertyId:long}")]
        public async Task<IActionResult> GetTransactionsByPropertyId(long propertyId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var transactions = await _rentalWorkflowService.GetTransactionsByPropertyIdAsync(propertyId, pageNumber, pageSize);
            return Ok(MapPagedResult(transactions));
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTransactionById(Guid id)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized("User ID not found in token");
            }

            var transaction = await _rentalWorkflowService.GetTransactionByIdAsync(id);
            if (transaction is null)
            {
                return NotFound(new ProblemDetails { Title = "Rental workflow record not found", Status = 404 });
            }

            if (transaction.OwnerId != userId && transaction.RenterId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "You do not have access to this rental workflow record." });
            }

            return Ok(MapTransaction(transaction));
        }

        [Authorize]
        [HttpGet("by-payment/{paymentRecordId:long}")]
        public async Task<IActionResult> GetTransactionByPaymentId(long paymentRecordId)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized("User ID not found in token");
            }

            var transaction = await _rentalWorkflowService.GetTransactionByPaymentRecordIdAsync(paymentRecordId);
            if (transaction is null)
            {
                return NotFound(new ProblemDetails { Title = "Rental workflow record not found", Status = 404 });
            }

            if (transaction.OwnerId != userId && transaction.RenterId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "You do not have access to this rental workflow record." });
            }

            return Ok(MapTransaction(transaction));
        }

        [Authorize]
        [HttpGet("by-contract/{contractId:long}")]
        public async Task<IActionResult> GetTransactionByContractId(long contractId)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized("User ID not found in token");
            }

            var transaction = await _rentalWorkflowService.GetTransactionByContractIdAsync(contractId);
            if (transaction is null)
            {
                return NotFound(new ProblemDetails { Title = "Rental workflow record not found", Status = 404 });
            }

            if (transaction.OwnerId != userId && transaction.RenterId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "You do not have access to this rental workflow record." });
            }

            return Ok(MapTransaction(transaction));
        }

        [Authorize]
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDto request)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized("User ID not found in token");
            }

            var domain = $"{Request.Scheme}://{Request.Host}";
            var successUrl = domain + (_configuration["Stripe:CheckoutSuccessPath"] ?? "/api/payments/success");
            var cancelUrl = domain + (_configuration["Stripe:CheckoutCancelPath"] ?? "/api/payments/cancel");

            try
            {
                var checkoutUrl = await _rentalWorkflowService.StartCheckoutAsync(request.PropertyId, userId, successUrl, cancelUrl);
                return Ok(new { url = checkoutUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to start workflow", error = ex.Message });
            }
        }

        private static PagedResult<RentalTransactionResponseDto> MapPagedResult(PagedResult<RentalTransaction> pagedResult)
        {
            return new PagedResult<RentalTransactionResponseDto>
            {
                Items = pagedResult.Items.Select(MapTransaction).ToList(),
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount,
                TotalPages = pagedResult.TotalPages
            };
        }

        private static RentalTransactionResponseDto MapTransaction(RentalTransaction transaction)
        {
            return new RentalTransactionResponseDto
            {
                Id = transaction.Id,
                RenterId = transaction.RenterId,
                OwnerId = transaction.OwnerId,
                PropertyId = transaction.PropertyId,
                StripeSessionId = transaction.StripeSessionId,
                PaymentId = transaction.PaymentId,
                ContractId = transaction.ContractId,
                Status = transaction.Status,
                PaymentStatus = transaction.PaymentStatus,
                CreatedAt = transaction.CreatedAt,
                CompletedAt = transaction.CompletedAt,
                PropertyTitle = transaction.Property?.Title,
                PropertyAddress = transaction.Property?.Address
            };
        }
    }
}
