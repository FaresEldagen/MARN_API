using MARN_API.Enums.Admin;

namespace MARN_API.Models
{
    public class AdminAnalyticsReport
    {
        public long Id { get; set; }
        public Guid GeneratedByAdminId { get; set; }
        public AdminAnalyticsReportScope Scope { get; set; }
        public AdminAnalyticsReportFormat Format { get; set; }
        public AdminAnalyticsReportPeriod RequestedPeriod { get; set; }
        public DateTime? FromUtc { get; set; }
        public DateTime? ToUtc { get; set; }
        public string Grouping { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string StoredFilePath { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        public virtual ApplicationUser GeneratedByAdmin { get; set; } = null!;
    }
}
