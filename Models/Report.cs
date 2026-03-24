using System;
using MARN_API.Enums;

namespace MARN_API.Models
{
    public class Report
    {
        public long Id { get; set; }
        public Guid ReporterId { get; set; }
        public Guid? ReviewerId { get; set; }
        public ReportableType ReportableType { get; set; }
        public long ReportableId { get; set; }

        public string Reason { get; set; } = string.Empty;
        public ReportStatus Status { get; set; } = ReportStatus.InReview;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }

        public virtual ApplicationUser Reporter { get; set; } = null!;
        public virtual Admin? Reviewer { get; set; }
    }
}



