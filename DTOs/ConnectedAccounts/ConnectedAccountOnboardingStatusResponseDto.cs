namespace MARN_API.DTOs.ConnectedAccounts
{
    public class ConnectedAccountOnboardingStatusResponseDto
    {
        public Guid ConnectedAccountId { get; set; }
        public Guid ApplicationUserId { get; set; }
        public string StripeAccountId { get; set; } = string.Empty;
        public bool IsOnboardingComplete { get; set; }
    }
}
