using MARN_API.DTOs.Admin;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IAdminDetailedStatsRepo
    {
        Task<AdminDetailedUsersResponseDto> GetUsersAsync(AdminDetailedUsersQueryDto query, DateTime? fromUtc, DateTime? toUtc, bool groupByDay);
        Task<AdminDetailedPropertiesResponseDto> GetPropertiesAsync(AdminDetailedPropertiesQueryDto query, DateTime? fromUtc, DateTime? toUtc);
        Task<AdminPropertyDetailsDto?> GetPropertyDetailsAsync(long propertyId);
        Task<AdminDetailedContractsResponseDto> GetContractsAsync(AdminDetailedContractsQueryDto query, DateTime? fromUtc, DateTime? toUtc);
        Task<AdminDetailedRevenueResponseDto> GetRevenueAsync(AdminDetailedRevenueQueryDto query, DateTime? fromUtc, DateTime? toUtc, bool groupByDay);
        Task<Property?> GetPropertyForAdminActionAsync(long propertyId);
        Task<Contract?> GetContractForAdminActionAsync(long contractId);
        Task SaveAdminContractChangesAsync();
    }
}
