using MARN_API.DTOs.Moderation;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IReportRepo
    {
        Task<AdminModerationQueueDto> GetAdminQueueAsync(AdminReportQueryDto query);
        Task<Report?> GetReportDetailsAsync(long reportId);
        Task<Report?> GetTrackedReportAsync(long reportId);

        Task<ApplicationUser?> GetUserTargetAsync(Guid userId);
        Task<Property?> GetPropertyTargetAsync(long propertyId);
        Task<Message?> GetMessageTargetAsync(Guid messageId);
        Task<PropertyFeedback?> GetPropertyCommentTargetAsync(long commentId);

        Task AddAsync(Report report);
        Task AddActionLogAsync(AdminActionLog actionLog);
        Task SaveChangesAsync();
        Task DeleteByReporterIdAsync(Guid userId);
    }
}
