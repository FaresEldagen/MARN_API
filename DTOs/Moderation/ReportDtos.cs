using MARN_API.DTOs.Common;
using MARN_API.Enums.Report;
using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.Moderation
{
    public class SubmitReportDto
    {
        public ReportableType ReportableType { get; set; }

        [Required(ErrorMessage = "Report target is required.")]
        public string ReportableTargetId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Report reason is required.")]
        [StringLength(2000, MinimumLength = 5, ErrorMessage = "Report reason must be between 5 and 2000 characters.")]
        public string Reason { get; set; } = string.Empty;
    }

    public class ReportSubmissionResultDto
    {
        public long ReportId { get; set; }
        public ReportStatus Status { get; set; }
        public string StatusDisplayName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class AdminReportQueryDto
    {
        public ReportStatus? Status { get; set; }
        public ReportableType? ReportableType { get; set; }
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class AdminReviewReportDto
    {
        public ReportStatus Status { get; set; }

        [StringLength(2000, ErrorMessage = "Review note cannot exceed 2000 characters.")]
        public string? Note { get; set; }

        public List<ReportModerationActionType>? ActionTypes { get; set; }
    }

    public class AdminModerationReportListItemDto
    {
        public long ReportId { get; set; }
        public ReportableType ReportableType { get; set; }
        public string ReportableTypeDisplayName { get; set; } = string.Empty;
        public ReportStatus Status { get; set; }
        public string StatusDisplayName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public ReportModerationActionType? ActionTaken { get; set; }
        public string ActionTakenDisplayName { get; set; } = string.Empty;
        public List<ReportModerationActionType> ActionsTaken { get; set; } = [];
        public List<string> ActionsTakenDisplayNames { get; set; } = [];
        public Guid ReporterId { get; set; }
        public string ReporterName { get; set; } = string.Empty;
        public Guid? ReviewerId { get; set; }
        public string? ReviewerName { get; set; }
        public string TargetLabel { get; set; } = string.Empty;
    }

    public class AdminModerationReportDetailsDto
    {
        public long ReportId { get; set; }
        public ReportableType ReportableType { get; set; }
        public string ReportableTypeDisplayName { get; set; } = string.Empty;
        public ReportStatus Status { get; set; }
        public string StatusDisplayName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string? ReviewerNote { get; set; }
        public ReportModerationActionType? ActionTaken { get; set; }
        public string ActionTakenDisplayName { get; set; } = string.Empty;
        public List<ReportModerationActionType> ActionsTaken { get; set; } = [];
        public List<string> ActionsTakenDisplayNames { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public Guid ReporterId { get; set; }
        public string ReporterName { get; set; } = string.Empty;
        public Guid? ReviewerId { get; set; }
        public string? ReviewerName { get; set; }
        public string ReportableTargetId { get; set; } = string.Empty;
        public AdminModerationTargetDetailsDto Target { get; set; } = new();
    }

    public class AdminModerationTargetDetailsDto
    {
        public bool Exists { get; set; }
        public bool IsHidden { get; set; }
        public bool IsDeletedOrInactive { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string? Preview { get; set; }
        public Guid? UserId { get; set; }
        public long? PropertyId { get; set; }
        public Guid? MessageId { get; set; }
        public long? PropertyCommentId { get; set; }
    }

    public class AdminModerationQueueDto
    {
        public PagedResult<AdminModerationReportListItemDto> Reports { get; set; } = new();
        public List<AdminReportStatusCountDto> StatusBreakdown { get; set; } = [];
        public List<AdminReportTypeCountDto> TypeBreakdown { get; set; } = [];
    }

    public class AdminReportStatusCountDto
    {
        public ReportStatus Status { get; set; }
        public string StatusDisplayName { get; set; } = string.Empty;
        public long Count { get; set; }
    }

    public class AdminReportTypeCountDto
    {
        public ReportableType ReportableType { get; set; }
        public string ReportableTypeDisplayName { get; set; } = string.Empty;
        public long Count { get; set; }
    }
}
