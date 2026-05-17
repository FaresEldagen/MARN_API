namespace MARN_API.Models
{
    public class AdminActionLog
    {
        public long Id { get; set; }
        public Guid AdminId { get; set; }
        public long? ReportId { get; set; }
        public string ActionType { get; set; } = string.Empty;
        public string TargetType { get; set; } = string.Empty;
        public Guid? TargetGuidId { get; set; }
        public long? TargetLongId { get; set; }
        public string? Reason { get; set; }
        public string? MetadataJson { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ApplicationUser Admin { get; set; } = null!;
        public virtual Report? Report { get; set; }
    }
}
