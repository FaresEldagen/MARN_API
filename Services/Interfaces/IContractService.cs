using MARN_API.DTOs.Common;
using MARN_API.DTOs.Contracts;
using MARN_API.Enums;
using MARN_API.Models;
using MARN_API.Services.Implementations;

namespace MARN_API.Services.Interfaces
{
    public interface IContractService
    {
        Task<PagedResult<Contract>> GetAllContractsAsync(int pageNumber, int pageSize);
        Task<PagedResult<Contract>> GetContractsByUserIdAsync(Guid userId, int pageNumber, int pageSize);
        Task<PagedResult<Contract>> GetContractsByPropertyIdAsync(long propertyId, int pageNumber, int pageSize);
        Task<Contract?> GetContractByIdAsync(long contractId);
        Task<string> HashContractAsync(IFormFile file);
        GeneratedContractPdfResult GenerateContractPdf(ContractPdfRequest request);
        Task<Contract> CreateWorkflowContractAsync(ContractPdfRequest request, Guid ownerId, Guid renterId, long propertyId, DateOnly? leaseStartDate = null, DateOnly? leaseEndDate = null, PaymentFrequency paymentFrequency = PaymentFrequency.OneTime);
        Task<Contract> SubmitContractAsync(IFormFile file, Guid ownerId, Guid renterId, long propertyId, DateOnly? leaseStartDate = null, DateOnly? leaseEndDate = null, PaymentFrequency paymentFrequency = PaymentFrequency.OneTime);
        Task<Contract> SubmitContractBytesAsync(byte[] fileBytes, string fileName, Guid ownerId, Guid renterId, long propertyId, DateOnly? leaseStartDate = null, DateOnly? leaseEndDate = null, PaymentFrequency paymentFrequency = PaymentFrequency.OneTime);
        Task<(bool match, string message, Contract? record)> VerifyContractAsync(IFormFile file, long contractId);
        Task<OpenTimestampsProofReader.OpenTimestampsProofExtractionResult> ExtractProofDataAsync(IFormFile anchoredOtsFile, IFormFile? originalFile);
    }
}
