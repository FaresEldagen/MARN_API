namespace MARN_API.DTOs.ConnectedAccounts
{
    public class ConnectedAccountOnboardingLinkResponseDto
    {
        public Guid ConnectedAccountId { get; set; }
        public Guid ApplicationUserId { get; set; }
        public string StripeAccountId { get; set; } = string.Empty;
        public string OnboardingUrl { get; set; } = string.Empty;
    }
}
