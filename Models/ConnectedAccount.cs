namespace MARN_API.Models
{
    public class ConnectedAccount
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ApplicationUserId { get; set; }
        public string? StripeAccountId { get; set; }
        public bool IsOnboardingComplete { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
