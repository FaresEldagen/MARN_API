namespace MARN_API.DTOs.ConnectedAccounts
{
    public class ConnectedAccountResponseDto
    {
        public Guid ConnectedAccountId { get; set; }
        public Guid ApplicationUserId { get; set; }
        public string? StripeAccountId { get; set; }
        public bool IsOnboardingComplete { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
    }
}
