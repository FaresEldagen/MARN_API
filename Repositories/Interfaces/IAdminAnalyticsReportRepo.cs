using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Common;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IAdminAnalyticsReportRepo
    {
        Task AddAsync(AdminAnalyticsReport report);
        Task<AdminAnalyticsReport?> GetByIdAsync(long reportId);
        Task<ApplicationUser?> GetAdminUserAsync(Guid adminId);
        Task<PagedResult<AdminAnalyticsReportListItemDto>> GetReportsAsync(AdminAnalyticsReportQueryDto query);
        Task SaveChangesAsync();
    }
}
