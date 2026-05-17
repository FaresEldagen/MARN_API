using MARN_API.DTOs.Admin;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IAdminDetailedStatsService
    {
        Task<ServiceResult<AdminDetailedUsersResponseDto>> GetUsersAsync(AdminDetailedUsersQueryDto query);
        Task<ServiceResult<AdminDetailedPropertiesResponseDto>> GetPropertiesAsync(AdminDetailedPropertiesQueryDto query);
        Task<ServiceResult<AdminDetailedPropertyListItemDto>> DeactivatePropertyAsync(long propertyId);
        Task<ServiceResult<AdminDetailedPropertyListItemDto>> RestorePropertyAsync(long propertyId);
        Task<ServiceResult<AdminDetailedContractsResponseDto>> GetContractsAsync(AdminDetailedContractsQueryDto query);
        Task<ServiceResult<AdminDetailedRevenueResponseDto>> GetRevenueAsync(AdminDetailedRevenueQueryDto query);
        Task<ServiceResult<AdminDetailedContractListItemDto>> CancelContractAsync(long contractId);
    }
}
