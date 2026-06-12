using MARN_API.DTOs.Common;
using MARN_API.Enums.Admin;
using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.Admin
{
    public class AdminAnalyticsReportGenerateRequestDto
    {
        public AdminAnalyticsReportScope Scope { get; set; } = AdminAnalyticsReportScope.Full;
        public AdminAnalyticsReportFormat Format { get; set; } = AdminAnalyticsReportFormat.Pdf;
        public AdminAnalyticsReportPeriod Period { get; set; } = AdminAnalyticsReportPeriod.ThisMonth;
        public DateTime? FromUtc { get; set; }
        public DateTime? ToUtc { get; set; }
    }

    public class AdminAnalyticsReportQueryDto
    {
        public AdminAnalyticsReportScope? Scope { get; set; }
        public AdminAnalyticsReportFormat? Format { get; set; }
        [Range(1, 9999, ErrorMessage = "Year must be between 1 and 9999.")]
        public int? Year { get; set; }
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
        public int? Month { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class AdminAnalyticsReportListItemDto
    {
        public long ReportId { get; set; }
        public AdminAnalyticsReportScope Scope { get; set; }
        public string ScopeDisplayName { get; set; } = string.Empty;
        public AdminAnalyticsReportFormat Format { get; set; }
        public string FormatDisplayName { get; set; } = string.Empty;
        public AdminAnalyticsReportPeriod RequestedPeriod { get; set; }
        public string RequestedPeriodDisplayName { get; set; } = string.Empty;
        public DateTime? FromUtc { get; set; }
        public DateTime? ToUtc { get; set; }
        public string Grouping { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
        public DateTime GeneratedAt { get; set; }
        public Guid GeneratedByAdminId { get; set; }
        public string GeneratedByAdminName { get; set; } = string.Empty;
    }

    public class AdminAnalyticsReportDetailsDto : AdminAnalyticsReportListItemDto
    {
        public string ContentType { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;
    }

    public class AdminAnalyticsReportDownloadDto
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public byte[] FileBytes { get; set; } = [];
    }
}
