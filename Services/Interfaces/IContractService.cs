using MARN_API.DTOs.Common;
using MARN_API.DTOs.Contracts;
using MARN_API.Enums;
using MARN_API.Models;
using MARN_API.Services.Implementations;

namespace MARN_API.Services.Interfaces
{
    public interface IContractService
    {
        Task<ServiceResult<ContractResponseDto>> CreateContractFromBookingAsync(Guid userId, long bookingRequestId);
        Task<ServiceResult<ContractResponseDto>> SignContractAsync(Guid userId, long contractId);

        Task<ServiceResult<ContractDetailsDto>> GetContractByIdAsync(Guid userId, long contractId);
        Task<ServiceResult<ContractVerificationResponseDto>> VerifyContractAsync(Guid userId, IFormFile file, long contractId);
        Task<ServiceResult<ContractFileDto>> DownloadOtsProofAsync(Guid userId, long contractId);
        Task<ServiceResult<ContractFileDto>> DownloadContractAsync(Guid userId, long contractId);

        Task<ServiceResult<string>> CancelContractAsync(Guid userId, long contractId);
    }
}
