namespace MARN_API.Models
{
    public class ExternalRecommendationResult
    {
        public bool Success { get; init; }
        public IReadOnlyList<long> PropertyIds { get; init; } = [];
        public string? FailureReason { get; init; }

        public static ExternalRecommendationResult Succeeded(IReadOnlyList<long> propertyIds)
        {
            return new ExternalRecommendationResult
            {
                Success = true,
                PropertyIds = propertyIds
            };
        }

        public static ExternalRecommendationResult Failed(string failureReason)
        {
            return new ExternalRecommendationResult
            {
                Success = false,
                FailureReason = failureReason
            };
        }
    }
}
