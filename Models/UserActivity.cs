namespace MARN_API.Models
{
    public class UserActivity
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public long? PropertyId { get; set; }
        public string UserActivityType { get; set; } = string.Empty;
        public string? Metadata { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ApplicationUser User { get; set; } = null!;
    }
}



