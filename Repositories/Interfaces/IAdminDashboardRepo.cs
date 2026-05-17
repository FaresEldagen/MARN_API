using MARN_API.DTOs.Admin;

namespace MARN_API.Repositories.Interfaces
{
    public interface IAdminDashboardRepo
    {
        Task<long> GetTotalNonAdminUsersAsync();
        Task<long> GetNonAdminUsersCreatedBetweenAsync(DateTime fromUtc, DateTime toUtc);
        Task<long> GetTotalPropertiesAsync();
        Task<long> GetPropertiesCreatedBetweenAsync(DateTime fromUtc, DateTime toUtc);
        Task<long> GetPendingUserVerificationsAsync();
        Task<long> GetPendingPropertyVerificationsAsync();
        Task<long> GetPendingUserVerificationsCreatedBetweenAsync(DateTime fromUtc, DateTime toUtc);
        Task<long> GetPendingPropertyVerificationsCreatedBetweenAsync(DateTime fromUtc, DateTime toUtc);
        Task<long> GetTotalContractsAsync();
        Task<long> GetContractsCreatedBetweenAsync(DateTime fromUtc, DateTime toUtc);
        Task<List<AdminContractStatusCountDto>> GetContractStatusCountsAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<decimal> GetTotalSalesAsync();
        Task<decimal> GetRevenueBetweenAsync(DateTime fromUtc, DateTime toUtc);
        Task<List<AdminMonthlyRevenuePointDto>> GetMonthlyRevenueAsync(DateTime fromUtc, DateTime toUtc);
    }
}
