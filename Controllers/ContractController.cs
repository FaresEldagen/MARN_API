using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MARN_API.DTOs.Common;
using MARN_API.DTOs.Contracts;
using MARN_API.Enums;
using MARN_API.Enums.Property;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;

namespace MARN_API.Controllers
{
    [ApiController]
    [Route("api/contracts")]
    public class ContractController : BaseController
    {
        private readonly IContractService _contractService;

        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }



        /// <summary>
        /// Creates a contract from an approved booking request.
        /// Only the property owner can create a contract for their property.
        /// </summary>
        /// <param name="bookingRequestId">The ID of the booking request to create a contract from.</param>
        /// <response code="201">Contract created successfully.</response>
        /// <response code="400">If the booking request is not in a valid state.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not the owner of the property.</response>
        /// <response code="404">If the booking request is not found.</response>
        /// <response code="409">If the property already has active contracts.</response>
        [Authorize(Roles = "Owner")]
        [HttpPost("create/{bookingRequestId:long}")]
        [ProducesResponseType(typeof(ApiResponseDto<ContractResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateContractFromBooking(long bookingRequestId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token.");

            var result = await _contractService.CreateContractFromBookingAsync(userId, bookingRequestId);
            return HandleServiceResult(result);
        }


        /// <summary>
        /// Allows a renter to sign a pending contract.
        /// This generates the contract PDF, computes its hash, and submits it to OpenTimestamps.
        /// </summary>
        /// <param name="contractId">The ID of the contract to sign.</param>
        /// <response code="200">Contract signed and anchoring process initiated.</response>
        /// <response code="400">If the contract is not in a Pending state.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not the renter of this contract.</response>
        /// <response code="404">If the contract is not found.</response>
        /// <response code="409">If the property already has active contracts.</response>
        [Authorize]
        [HttpPost("{contractId:long}/sign")]
        [ProducesResponseType(typeof(ApiResponseDto<ContractResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SignContract(long contractId)
        {
            if (!TryGetUserId(out var userId)) 
                return Unauthorized("User ID not found in token.");

            var result = await _contractService.SignContractAsync(userId, contractId);
            return HandleServiceResult(result);
        }


        /// <summary>
        /// Returns a single contract record by ID.
        /// </summary>
        /// <param name="contractId">The ID of the contract to retrieve.</param>
        /// <response code="200">
        /// Returns the contract details:
        /// - Contract status and ID
        /// - Rental duration (formatted string)
        /// - Property details (ID, name, address, price, etc.)
        /// - Renter details (ID, name, email, profile image)
        /// - Owner details (ID, name, email, profile image)
        /// - Lease start and end dates
        /// - Total contract value
        /// </response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not the renter, owner, or an admin.</response>
        /// <response code="404">If the contract is not found.</response>
        [Authorize]
        [HttpGet("{contractId:long}")]
        [ProducesResponseType(typeof(ApiResponseDto<ContractDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetContract(long contractId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token.");

            var result = await _contractService.GetContractByIdAsync(userId, contractId);
            return HandleServiceResult(result);
        }


        /// <summary>
        /// Downloads the stored OpenTimestamps proof file for a contract.
        /// </summary>
        /// <param name="contractId">The ID of the contract whose proof is being downloaded.</param>
        /// <response code="200">Returns the .ots proof file.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not authorized to access this contract.</response>
        /// <response code="404">If the contract or proof file is not found.</response>
        [Authorize]
        [HttpGet("{contractId:long}/proof")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadOtsProof(long contractId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token.");

            var result = await _contractService.DownloadOtsProofAsync(userId, contractId);

            if (!result.Success)
                return HandleServiceResult(result);

            return File(result.Data!.FileBytes, result.Data.ContentType, result.Data.FileName);
        }


        /// <summary>
        /// Downloads the stored contract PDF file.
        /// </summary>
        /// <param name="contractId">The ID of the contract to download.</param>
        /// <response code="200">Returns the contract PDF file.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not authorized to access this contract.</response>
        /// <response code="404">If the contract or PDF file is not found.</response>
        [Authorize]
        [HttpGet("{contractId:long}/download")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadContract(long contractId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token.");

            var result = await _contractService.DownloadContractAsync(userId, contractId);

            if (!result.Success)
                return HandleServiceResult(result);

            return File(result.Data!.FileBytes, result.Data.ContentType, result.Data.FileName);
        }


        /// <summary>
        /// Verifies an uploaded contract file matches the stored hash.
        /// </summary>
        /// <param name="file">The PDF file to verify.</param>
        /// <param name="contractId">The ID of the contract to verify against.</param>
        /// <response code="200">
        /// Returns verification results:
        /// - Match (boolean)
        /// - Message explaining the status
        /// - Current contract status
        /// - Current anchoring status
        /// </response>
        /// <response code="400">If the file is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not authorized to access this contract.</response>
        /// <response code="404">If the contract is not found.</response>
        [Authorize]
        [HttpPost("verify")]
        [ProducesResponseType(typeof(ApiResponseDto<ContractVerificationResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> VerifyContract(IFormFile file, [FromQuery] long contractId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token.");

            var result = await _contractService.VerifyContractAsync(userId, file, contractId);
            return HandleServiceResult(result);
        }


        /// <summary>
        /// Cancels and deletes a pending contract.
        /// </summary>
        /// <param name="contractId">The ID of the contract to cancel.</param>
        /// <response code="200">Contract cancelled successfully.</response>
        /// <response code="400">If the contract is already signed or anchored.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not the renter or owner.</response>
        /// <response code="404">If the contract is not found.</response>
        [Authorize]
        [HttpDelete("cancel/{contractId:long}")]
        [ProducesResponseType(typeof(ApiResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelContract(long contractId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token.");

            var result = await _contractService.CancelContractAsync(userId, contractId);
            return HandleServiceResult(result);
        }
    }
}
