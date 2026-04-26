using MARN_API.DTOs.Common;
using MARN_API.DTOs.Contracts;
using MARN_API.Enums;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class ContractService : IContractService
    {
        private readonly IWorkflowContractRepo _repo;
        private readonly HashingService _hashingService;
        private readonly OpenTimestampsService _openTimestampsService;
        private readonly OpenTimestampsProofReader _proofReader;
        private readonly ContractPdfGenerator _contractPdfGenerator;

        public ContractService(
            IWorkflowContractRepo repo,
            HashingService hashingService,
            OpenTimestampsService openTimestampsService,
            OpenTimestampsProofReader proofReader,
            ContractPdfGenerator contractPdfGenerator)
        {
            _repo = repo;
            _hashingService = hashingService;
            _openTimestampsService = openTimestampsService;
            _proofReader = proofReader;
            _contractPdfGenerator = contractPdfGenerator;
        }

        public Task<PagedResult<Contract>> GetAllContractsAsync(int pageNumber, int pageSize)
        {
            return _repo.GetAllAsync(pageNumber, pageSize);
        }

        public Task<PagedResult<Contract>> GetContractsByUserIdAsync(Guid userId, int pageNumber, int pageSize)
        {
            return _repo.GetByUserIdAsync(userId, pageNumber, pageSize);
        }

        public Task<PagedResult<Contract>> GetContractsByPropertyIdAsync(long propertyId, int pageNumber, int pageSize)
        {
            return _repo.GetByPropertyIdAsync(propertyId, pageNumber, pageSize);
        }

        public Task<Contract?> GetContractByIdAsync(long contractId)
        {
            return _repo.GetByIdAsync(contractId);
        }

        public async Task<string> HashContractAsync(IFormFile file)
        {
            await using var stream = file.OpenReadStream();
            return await _hashingService.ComputeSha256HashAsync(stream);
        }

        public GeneratedContractPdfResult GenerateContractPdf(ContractPdfRequest request)
        {
            return _contractPdfGenerator.Generate(request);
        }

        public async Task<Contract> CreateWorkflowContractAsync(ContractPdfRequest request, Guid ownerId, Guid renterId, long propertyId, DateOnly? leaseStartDate = null, DateOnly? leaseEndDate = null, PaymentFrequency paymentFrequency = PaymentFrequency.OneTime)
        {
            var contract = new Contract
            {
                ContractNumber = "PENDING",
                OwnerId = ownerId,
                RenterId = renterId,
                PropertyId = propertyId,
                LeaseStartDate = leaseStartDate,
                LeaseEndDate = leaseEndDate,
                SubmittedAt = DateTime.UtcNow,
                Status = ContractStatus.Pending,
                AnchoringStatus = ContractAnchoringStatus.Pending,
                PaymentFrequency = paymentFrequency
            };

            await _repo.AddAsync(contract);

            request.ContractNumber = contract.Id.ToString();
            var pdfResult = _contractPdfGenerator.Generate(request);

            await using var stream = new MemoryStream(pdfResult.Content);
            var hash = await _hashingService.ComputeSha256HashAsync(stream);

            var otsFileBytes = await _openTimestampsService.SubmitHashAsync(hash);
            var proofData = _proofReader.Extract(otsFileBytes);

            contract.ContractNumber = contract.Id.ToString();
            contract.FileName = pdfResult.FileName;
            contract.FileBytes = pdfResult.Content;
            contract.Hash = hash;
            contract.SubmittedAt = DateTime.UtcNow;
            contract.Status = ContractStatus.Active;
            contract.AnchoringStatus = ContractAnchoringStatus.Pending;
            contract.OtsFileBytes = otsFileBytes;
            contract.TransactionId = proofData.TransactionIds.FirstOrDefault();
            contract.MerkleRoot = proofData.MerkleRoots.FirstOrDefault();

            await _repo.UpdateAsync(contract);
            return contract;
        }

        public async Task<Contract> SubmitContractAsync(IFormFile file, Guid ownerId, Guid renterId, long propertyId, DateOnly? leaseStartDate = null, DateOnly? leaseEndDate = null, PaymentFrequency paymentFrequency = PaymentFrequency.OneTime)
        {
            await using var stream = file.OpenReadStream();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return await SubmitContractBytesAsync(ms.ToArray(), file.FileName, ownerId, renterId, propertyId, leaseStartDate, leaseEndDate, paymentFrequency);
        }

        public async Task<Contract> SubmitContractBytesAsync(byte[] fileBytes, string fileName, Guid ownerId, Guid renterId, long propertyId, DateOnly? leaseStartDate = null, DateOnly? leaseEndDate = null, PaymentFrequency paymentFrequency = PaymentFrequency.OneTime)
        {
            await using var stream = new MemoryStream(fileBytes);
            var hash = await _hashingService.ComputeSha256HashAsync(stream);

            var otsFileBytes = await _openTimestampsService.SubmitHashAsync(hash);
            var proofData = _proofReader.Extract(otsFileBytes);

            var contract = new Contract
            {
                ContractNumber = $"MARN-{DateTime.UtcNow:yyyyMMddHHmmss}-{propertyId}",
                OwnerId = ownerId,
                RenterId = renterId,
                PropertyId = propertyId,
                LeaseStartDate = leaseStartDate,
                LeaseEndDate = leaseEndDate,
                FileName = fileName,
                FileBytes = fileBytes,
                Hash = hash,
                SubmittedAt = DateTime.UtcNow,
                Status = ContractStatus.Active,
                AnchoringStatus = ContractAnchoringStatus.Pending,
                OtsFileBytes = otsFileBytes,
                TransactionId = proofData.TransactionIds.FirstOrDefault(),
                MerkleRoot = proofData.MerkleRoots.FirstOrDefault(),
                PaymentFrequency = paymentFrequency
            };

            await _repo.AddAsync(contract);
            return contract;
        }

        public async Task<(bool match, string message, Contract? record)> VerifyContractAsync(IFormFile file, long contractId)
        {
            var record = await _repo.GetByIdAsync(contractId);
            if (record is null)
            {
                return (false, "Contract not found", null);
            }

            await using var stream = file.OpenReadStream();
            var hash = await _hashingService.ComputeSha256HashAsync(stream);

            if (!string.Equals(record.Hash, hash, StringComparison.OrdinalIgnoreCase))
            {
                return (false, "Contract has been tampered with.", record);
            }

            var message = record.AnchoringStatus == ContractAnchoringStatus.Pending
                ? "Original contract verified. Blockchain anchoring is still in progress."
                : "Original contract verified and anchored on Bitcoin.";

            return (true, message, record);
        }

        public async Task<OpenTimestampsProofReader.OpenTimestampsProofExtractionResult> ExtractProofDataAsync(IFormFile anchoredOtsFile, IFormFile? originalFile)
        {
            await using var anchoredOtsStream = anchoredOtsFile.OpenReadStream();
            using var anchoredOtsMemory = new MemoryStream();
            await anchoredOtsStream.CopyToAsync(anchoredOtsMemory);

            byte[]? originalFileBytes = null;
            if (originalFile is not null)
            {
                await using var originalFileStream = originalFile.OpenReadStream();
                using var originalFileMemory = new MemoryStream();
                await originalFileStream.CopyToAsync(originalFileMemory);
                originalFileBytes = originalFileMemory.ToArray();
            }

            return _proofReader.Extract(anchoredOtsMemory.ToArray(), originalFileBytes);
        }
    }
}
