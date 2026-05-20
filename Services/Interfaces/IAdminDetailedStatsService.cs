using MARN_API.DTOs.Admin;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IAdminDetailedStatsService
    {
        Task<ServiceResult<AdminDetailedUsersResponseDto>> GetUsersAsync(AdminDetailedUsersQueryDto query);
        Task<ServiceResult<AdminDetailedPropertiesResponseDto>> GetPropertiesAsync(AdminDetailedPropertiesQueryDto query);
        Task<ServiceResult<AdminPropertyDetailsDto>> GetPropertyDetailsAsync(long propertyId);
        Task<ServiceResult<bool>> DeletePropertyAsync(long propertyId);
        Task<ServiceResult<AdminDetailedPropertyListItemDto>> RestoreDeletedPropertyAsync(long propertyId);
        Task<ServiceResult<AdminDetailedContractsResponseDto>> GetContractsAsync(AdminDetailedContractsQueryDto query);
        Task<ServiceResult<AdminDetailedRevenueResponseDto>> GetRevenueAsync(AdminDetailedRevenueQueryDto query);
        Task<ServiceResult<AdminDetailedContractListItemDto>> CancelContractAsync(long contractId);
    }
}
