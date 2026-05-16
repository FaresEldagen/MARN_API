using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Common;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IAdminAnalyticsReportService
    {
        Task<ServiceResult<AdminAnalyticsReportDetailsDto>> GenerateAsync(Guid adminId, AdminAnalyticsReportGenerateRequestDto request);
        Task<ServiceResult<PagedResult<AdminAnalyticsReportListItemDto>>> GetReportsAsync(AdminAnalyticsReportQueryDto query);
        Task<ServiceResult<AdminAnalyticsReportDetailsDto>> GetReportAsync(long reportId);
        Task<ServiceResult<AdminAnalyticsReportDownloadDto>> DownloadAsync(long reportId);
    }
}
