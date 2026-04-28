//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using MARN_API.DTOs.Common;
//using MARN_API.DTOs.Contracts;
//using MARN_API.Models;
//using MARN_API.Services.Interfaces;

//namespace MARN_API.Controllers
//{
//    [ApiController]
//    [Route("api/contracts")]
//    public class ContractController : BaseController
//    {
//        private readonly IContractService _contractService;

//        public ContractController(IContractService contractService)
//        {
//            _contractService = contractService;
//        }

//        /// <summary>
//        /// Returns all contract records for testing and operational inspection.
//        /// </summary>
//        /// <response code="200">Returns a paged list of contract records.</response>
//        [HttpGet]
//        [ProducesResponseType(typeof(PagedResult<ContractResponseDto>), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public async Task<IActionResult> GetAllContracts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
//        {
//            if (pageNumber < 1 || pageSize < 1)
//            {
//                return BadRequest(new { error = "pageNumber and pageSize must be greater than 0." });
//            }

//            var contracts = await _contractService.GetAllContractsAsync(pageNumber, pageSize);
//            return Ok(MapPagedResult(contracts));
//        }

//        /// <summary>
//        /// Returns contract records that belong to the currently authenticated user.
//        /// </summary>
//        /// <response code="200">Returns a paged list of the current user's contracts.</response>
//        /// <response code="401">If the user is not authenticated.</response>
//        [Authorize]
//        [HttpGet("my")]
//        [ProducesResponseType(typeof(PagedResult<ContractResponseDto>), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        public async Task<IActionResult> GetMyContracts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
//        {
//            if (!TryGetUserId(out var userId))
//            {
//                return Unauthorized("User ID not found in token");
//            }

//            var contracts = await _contractService.GetContractsByUserIdAsync(userId, pageNumber, pageSize);
//            return Ok(MapPagedResult(contracts));
//        }

//        /// <summary>
//        /// Returns contract records associated with a specific property.
//        /// </summary>
//        /// <response code="200">Returns a paged list of contracts for the specified property.</response>
//        [HttpGet("by-property/{propertyId:long}")]
//        [ProducesResponseType(typeof(PagedResult<ContractResponseDto>), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetContractsByPropertyId(long propertyId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
//        {
//            var contracts = await _contractService.GetContractsByPropertyIdAsync(propertyId, pageNumber, pageSize);
//            return Ok(MapPagedResult(contracts));
//        }

//        /// <summary>
//        /// Returns contract records associated with a specific user ID, whether as owner or renter.
//        /// </summary>
//        /// <response code="200">Returns a paged list of contracts for the specified user.</response>
//        [HttpGet("by-user/{userId:guid}")]
//        [ProducesResponseType(typeof(PagedResult<ContractResponseDto>), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetContractsByUserId(Guid userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
//        {
//            var contracts = await _contractService.GetContractsByUserIdAsync(userId, pageNumber, pageSize);
//            return Ok(MapPagedResult(contracts));
//        }

//        /// <summary>
//        /// Returns a single contract record by its identifier.
//        /// </summary>
//        /// <response code="200">Returns the requested contract record.</response>
//        /// <response code="404">If the contract is not found.</response>
//        [HttpGet("{contractId:long}")]
//        [ProducesResponseType(typeof(ContractResponseDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> GetContract(long contractId)
//        {
//            var contract = await _contractService.GetContractByIdAsync(contractId);
//            if (contract is null)
//            {
//                return NotFound(new ProblemDetails { Title = "Contract not found", Status = 404 });
//            }

//            return Ok(MapContract(contract));
//        }

//        /// <summary>
//        /// Computes the SHA-256 hash of an uploaded contract file.
//        /// </summary>
//        /// <response code="200">Returns the computed SHA-256 hash.</response>
//        /// <response code="400">If no file was uploaded.</response>
//        [HttpPost("hash")]
//        [ProducesResponseType(typeof(ContractHashResponseDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public async Task<IActionResult> HashContract(IFormFile file)
//        {
//            if (file is null || file.Length == 0)
//            {
//                return BadRequest(new ProblemDetails { Title = "No file uploaded.", Status = 400 });
//            }

//            var hash = await _contractService.HashContractAsync(file);
//            return Ok(new ContractHashResponseDto { Hash = hash });
//        }

//        /// <summary>
//        /// Generates a rental contract PDF from the supplied request payload.
//        /// </summary>
//        /// <response code="200">Returns the generated PDF file.</response>
//        /// <response code="400">If the request payload is invalid.</response>
//        [HttpPost("generate-pdf")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public IActionResult GenerateContractPdf([FromBody] ContractPdfRequest request)
//        {
//            var result = _contractService.GenerateContractPdf(request);
//            return File(result.Content, "application/pdf", result.FileName);
//        }

//        /// <summary>
//        /// Verifies that an uploaded contract file matches the stored contract hash.
//        /// </summary>
//        /// <response code="200">Returns the verification result.</response>
//        /// <response code="401">If the user is not authenticated.</response>
//        /// <response code="403">If the user does not have access to the contract.</response>
//        /// <response code="404">If the contract is not found.</response>
//        [Authorize]
//        [HttpPost("verify")]
//        [ProducesResponseType(typeof(ContractVerificationResponseDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> VerifyContract(IFormFile file, [FromQuery] long contractId)
//        {
//            if (!TryGetUserId(out var userId))
//            {
//                return Unauthorized("User ID not found in token");
//            }

//            var contract = await _contractService.GetContractByIdAsync(contractId);
//            if (contract is null)
//            {
//                return NotFound(new ProblemDetails { Title = "Contract not found", Status = 404 });
//            }

//            if (contract.OwnerId != userId && contract.RenterId != userId)
//            {
//                return StatusCode(StatusCodes.Status403Forbidden, new { message = "You do not have access to this contract." });
//            }

//            var (match, message, record) = await _contractService.VerifyContractAsync(file, contractId);
//            if (record is null)
//            {
//                return NotFound(new ProblemDetails { Title = message, Status = 404 });
//            }

//            return Ok(new ContractVerificationResponseDto
//            {
//                Match = match,
//                Message = message,
//                Status = record.Status,
//                AnchoringStatus = record.AnchoringStatus,
//                AnchoredAt = record.AnchoredAt
//            });
//        }

//        /// <summary>
//        /// Extracts proof details from an OpenTimestamps proof file.
//        /// </summary>
//        /// <response code="200">Returns parsed proof information from the supplied .ots file.</response>
//        /// <response code="400">If the .ots file is missing or invalid.</response>
//        [HttpPost("extract-proof-data")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public async Task<IActionResult> ExtractProofData(IFormFile anchoredOtsFile, IFormFile? originalFile = null)
//        {
//            if (anchoredOtsFile is null || anchoredOtsFile.Length == 0)
//            {
//                return BadRequest(new ProblemDetails { Title = "No anchored .ots file uploaded.", Status = 400 });
//            }

//            try
//            {
//                var result = await _contractService.ExtractProofDataAsync(anchoredOtsFile, originalFile);
//                return Ok(result);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new ProblemDetails
//                {
//                    Title = "Could not extract proof data from the supplied .ots file.",
//                    Detail = ex.Message,
//                    Status = 400
//                });
//            }
//        }

//        /// <summary>
//        /// Downloads the stored OpenTimestamps proof file for a contract.
//        /// </summary>
//        /// <response code="200">Returns the stored .ots proof file.</response>
//        /// <response code="401">If the user is not authenticated.</response>
//        /// <response code="403">If the user does not have access to the contract.</response>
//        /// <response code="404">If the proof file is not found.</response>
//        [Authorize]
//        [HttpGet("{contractId:long}/proof")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> DownloadOtsProof(long contractId)
//        {
//            if (!TryGetUserId(out var userId))
//            {
//                return Unauthorized("User ID not found in token");
//            }

//            var contract = await _contractService.GetContractByIdAsync(contractId);
//            if (contract is null || contract.OtsFileBytes is null)
//            {
//                return NotFound(new ProblemDetails { Title = "Proof not found", Status = 404 });
//            }

//            if (contract.OwnerId != userId && contract.RenterId != userId)
//            {
//                return StatusCode(StatusCodes.Status403Forbidden, new { message = "You do not have access to this contract." });
//            }

//            return File(contract.OtsFileBytes, "application/octet-stream", $"{contract.FileName}.ots");
//        }

//        /// <summary>
//        /// Downloads the stored contract PDF file.
//        /// </summary>
//        /// <response code="200">Returns the stored contract PDF file.</response>
//        /// <response code="401">If the user is not authenticated.</response>
//        /// <response code="403">If the user does not have access to the contract.</response>
//        /// <response code="404">If the contract file is not found.</response>
//        [Authorize]
//        [HttpGet("{contractId:long}/download")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> DownloadContract(long contractId)
//        {
//            if (!TryGetUserId(out var userId))
//            {
//                return Unauthorized("User ID not found in token");
//            }

//            var contract = await _contractService.GetContractByIdAsync(contractId);
//            if (contract is null || contract.FileBytes is null)
//            {
//                return NotFound(new ProblemDetails { Title = "Contract file not found", Status = 404 });
//            }

//            if (contract.OwnerId != userId && contract.RenterId != userId)
//            {
//                return StatusCode(StatusCodes.Status403Forbidden, new { message = "You do not have access to this contract." });
//            }

//            return File(contract.FileBytes, "application/pdf", contract.FileName);
//        }

//        private static PagedResult<ContractResponseDto> MapPagedResult(PagedResult<Contract> pagedResult)
//        {
//            return new PagedResult<ContractResponseDto>
//            {
//                Items = pagedResult.Items.Select(MapContract).ToList(),
//                PageNumber = pagedResult.PageNumber,
//                PageSize = pagedResult.PageSize,
//                TotalCount = pagedResult.TotalCount,
//                TotalPages = pagedResult.TotalPages
//            };
//        }

//        private static ContractResponseDto MapContract(Contract contract)
//        {
//            return new ContractResponseDto
//            {
//                Id = contract.Id,
//                PropertyId = contract.PropertyId,
//                RenterId = contract.RenterId,
//                OwnerId = contract.OwnerId,
//                LeaseStartDate = contract.LeaseStartDate,
//                LeaseEndDate = contract.LeaseEndDate,
//                FileName = contract.FileName,
//                Hash = contract.Hash,
//                SubmittedAt = contract.SubmittedAt,
//                AnchoredAt = contract.AnchoredAt,
//                TransactionId = contract.TransactionId,
//                MerkleRoot = contract.MerkleRoot,
//                Status = contract.Status,
//                AnchoringStatus = contract.AnchoringStatus,
//                CreatedAt = contract.CreatedAt,
//                UpdatedAt = contract.UpdatedAt
//            };
//        }
//    }
//}
