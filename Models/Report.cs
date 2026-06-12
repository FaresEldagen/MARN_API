using System;
using MARN_API.Enums.Report;

namespace MARN_API.Models
{
    public class Report
    {
        public long Id { get; set; }
        public Guid ReporterId { get; set; }
        public Guid? ReviewerId { get; set; }
        public ReportableType ReportableType { get; set; }
        public long? ReportableId { get; set; }
        public Guid? ReportableGuidId { get; set; }

        public string Reason { get; set; } = string.Empty;
        public ReportStatus Status { get; set; } = ReportStatus.InReview;
        public string? ReviewerNote { get; set; }
        public ReportModerationActionType? ActionTaken { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }

        public virtual ApplicationUser Reporter { get; set; } = null!;
        public virtual ApplicationUser? Reviewer { get; set; }
        public virtual ICollection<AdminActionLog> ActionLogs { get; set; } = new HashSet<AdminActionLog>();
    }
}



