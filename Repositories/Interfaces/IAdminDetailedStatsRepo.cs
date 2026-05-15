using MARN_API.DTOs.Admin;

namespace MARN_API.Repositories.Interfaces
{
    public interface IAdminDetailedStatsRepo
    {
        Task<AdminDetailedUsersResponseDto> GetUsersAsync(AdminDetailedUsersQueryDto query, DateTime? fromUtc, DateTime? toUtc, bool groupByDay);
        Task<AdminDetailedPropertiesResponseDto> GetPropertiesAsync(AdminDetailedPropertiesQueryDto query, DateTime? fromUtc, DateTime? toUtc);
        Task<AdminDetailedContractsResponseDto> GetContractsAsync(AdminDetailedContractsQueryDto query, DateTime? fromUtc, DateTime? toUtc);
        Task<AdminDetailedRevenueResponseDto> GetRevenueAsync(AdminDetailedRevenueQueryDto query, DateTime? fromUtc, DateTime? toUtc, bool groupByDay);
    }
}
