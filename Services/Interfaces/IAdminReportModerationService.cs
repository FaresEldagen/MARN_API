using MARN_API.DTOs.Moderation;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IAdminReportModerationService
    {
        Task<ServiceResult<AdminModerationQueueDto>> GetReportsAsync(AdminReportQueryDto query);
        Task<ServiceResult<AdminModerationReportDetailsDto>> GetReportDetailsAsync(long reportId);
        Task<ServiceResult<AdminModerationReportDetailsDto>> ReviewReportAsync(Guid adminId, long reportId, AdminReviewReportDto request);
    }
}
