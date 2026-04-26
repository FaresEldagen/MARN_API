using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MARN_API.DTOs.Common;
using MARN_API.DTOs.Contracts;
using MARN_API.Models;
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

        [HttpGet]
        public async Task<IActionResult> GetAllContracts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest(new { error = "pageNumber and pageSize must be greater than 0." });
            }

            var contracts = await _contractService.GetAllContractsAsync(pageNumber, pageSize);
            return Ok(MapPagedResult(contracts));
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyContracts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized("User ID not found in token");
            }

            var contracts = await _contractService.GetContractsByUserIdAsync(userId, pageNumber, pageSize);
            return Ok(MapPagedResult(contracts));
        }

        [HttpGet("by-property/{propertyId:long}")]
        public async Task<IActionResult> GetContractsByPropertyId(long propertyId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var contracts = await _contractService.GetContractsByPropertyIdAsync(propertyId, pageNumber, pageSize);
            return Ok(MapPagedResult(contracts));
        }

        [Authorize]
        [HttpGet("{contractId:long}")]
        public async Task<IActionResult> GetContract(long contractId)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized("User ID not found in token");
            }

            var contract = await _contractService.GetContractByIdAsync(contractId);
            if (contract is null)
            {
                return NotFound(new ProblemDetails { Title = "Contract not found", Status = 404 });
            }

            if (contract.OwnerId != userId && contract.RenterId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "You do not have access to this contract." });
            }

            return Ok(MapContract(contract));
        }

        [HttpPost("hash")]
        public async Task<IActionResult> HashContract(IFormFile file)
        {
            if (file is null || file.Length == 0)
            {
                return BadRequest(new ProblemDetails { Title = "No file uploaded.", Status = 400 });
            }

            var hash = await _contractService.HashContractAsync(file);
            return Ok(new { hash });
        }

        [HttpPost("generate-pdf")]
        public IActionResult GenerateContractPdf([FromBody] ContractPdfRequest request)
        {
            var result = _contractService.GenerateContractPdf(request);
            return File(result.Content, "application/pdf", result.FileName);
        }

        [Authorize]
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyContract(IFormFile file, [FromQuery] long contractId)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized("User ID not found in token");
            }

            var contract = await _contractService.GetContractByIdAsync(contractId);
            if (contract is null)
            {
                return NotFound(new ProblemDetails { Title = "Contract not found", Status = 404 });
            }

            if (contract.OwnerId != userId && contract.RenterId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "You do not have access to this contract." });
            }

            var (match, message, record) = await _contractService.VerifyContractAsync(file, contractId);
            if (record is null)
            {
                return NotFound(new ProblemDetails { Title = message, Status = 404 });
            }

            return Ok(new
            {
                match,
                message,
                status = record.Status,
                anchoringStatus = record.AnchoringStatus,
                anchoredAt = record.AnchoredAt
            });
        }

        [HttpPost("extract-proof-data")]
        public async Task<IActionResult> ExtractProofData(IFormFile anchoredOtsFile, IFormFile? originalFile = null)
        {
            if (anchoredOtsFile is null || anchoredOtsFile.Length == 0)
            {
                return BadRequest(new ProblemDetails { Title = "No anchored .ots file uploaded.", Status = 400 });
            }

            try
            {
                var result = await _contractService.ExtractProofDataAsync(anchoredOtsFile, originalFile);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Could not extract proof data from the supplied .ots file.",
                    Detail = ex.Message,
                    Status = 400
                });
            }
        }

        [Authorize]
        [HttpGet("{contractId:long}/proof")]
        public async Task<IActionResult> DownloadOtsProof(long contractId)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized("User ID not found in token");
            }

            var contract = await _contractService.GetContractByIdAsync(contractId);
            if (contract is null || contract.OtsFileBytes is null)
            {
                return NotFound(new ProblemDetails { Title = "Proof not found", Status = 404 });
            }

            if (contract.OwnerId != userId && contract.RenterId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "You do not have access to this contract." });
            }

            return File(contract.OtsFileBytes, "application/octet-stream", $"{contract.FileName}.ots");
        }

        [Authorize]
        [HttpGet("{contractId:long}/download")]
        public async Task<IActionResult> DownloadContract(long contractId)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized("User ID not found in token");
            }

            var contract = await _contractService.GetContractByIdAsync(contractId);
            if (contract is null || contract.FileBytes is null)
            {
                return NotFound(new ProblemDetails { Title = "Contract file not found", Status = 404 });
            }

            if (contract.OwnerId != userId && contract.RenterId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "You do not have access to this contract." });
            }

            return File(contract.FileBytes, "application/pdf", contract.FileName);
        }

        private static PagedResult<ContractResponseDto> MapPagedResult(PagedResult<Contract> pagedResult)
        {
            return new PagedResult<ContractResponseDto>
            {
                Items = pagedResult.Items.Select(MapContract).ToList(),
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount,
                TotalPages = pagedResult.TotalPages
            };
        }

        private static ContractResponseDto MapContract(Contract contract)
        {
            return new ContractResponseDto
            {
                Id = contract.Id,
                PropertyId = contract.PropertyId,
                RenterId = contract.RenterId,
                OwnerId = contract.OwnerId,
                LeaseStartDate = contract.LeaseStartDate,
                LeaseEndDate = contract.LeaseEndDate,
                FileName = contract.FileName,
                Hash = contract.Hash,
                SubmittedAt = contract.SubmittedAt,
                AnchoredAt = contract.AnchoredAt,
                TransactionId = contract.TransactionId,
                MerkleRoot = contract.MerkleRoot,
                Status = contract.Status,
                AnchoringStatus = contract.AnchoringStatus,
                CreatedAt = contract.CreatedAt,
                UpdatedAt = contract.UpdatedAt
            };
        }
    }
}
