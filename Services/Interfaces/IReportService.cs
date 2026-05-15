using MARN_API.DTOs.Moderation;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IReportService
    {
        Task<ServiceResult<ReportSubmissionResultDto>> SubmitReportAsync(Guid reporterId, SubmitReportDto request);
    }
}
