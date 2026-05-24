using MARN_API.DTOs.Property;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class HomepageService : IHomepageService
    {
        private const int RecommendationCount = 8;
        private readonly IPropertyRepo _propertyRepo;
        private readonly IExternalPropertyAiClient _externalPropertyAiClient;
        private readonly ILogger<HomepageService> _logger;

        public HomepageService(
            IPropertyRepo propertyRepo,
            IExternalPropertyAiClient externalPropertyAiClient,
            ILogger<HomepageService> logger)
        {
            _propertyRepo = propertyRepo;
            _externalPropertyAiClient = externalPropertyAiClient;
            _logger = logger;
        }


        public async Task<ServiceResult<PropertySearchResultDto>> GetRecommendedPropertiesAsync(Guid? userId)
        {
            _logger.LogInformation("Retrieving recommended properties for userId: {userId}", userId);

            if (!userId.HasValue)
            {
                var anonymousFallback = await _propertyRepo.GetTopViewedPublicPropertyCardsAsync(
                    RecommendationCount,
                    null,
                    null);

                _logger.LogInformation(
                    "Recommendations requested anonymously. Returned {Count} top-viewed properties without calling the external recommendation endpoint.",
                    anonymousFallback.Count);

                return ServiceResult<PropertySearchResultDto>.Ok(new PropertySearchResultDto
                {
                    Items = anonymousFallback,
                    TotalCount = anonymousFallback.Count,
                    Page = 1,
                    PageSize = RecommendationCount
                });
            }

            var externalResult = await _externalPropertyAiClient.GetRecommendedPropertyIdsAsync(userId.Value);
            var recommendedIds = DeduplicatePreservingOrder(externalResult.PropertyIds);

            var recommendedProperties = externalResult.Success && recommendedIds.Count > 0
                ? await _propertyRepo.GetPublicPropertyCardsByIdsAsync(recommendedIds, userId.Value)
                : [];

            var finalProperties = recommendedProperties
                .Take(RecommendationCount)
                .ToList();

            var fallbackPropertiesAdded = 0;
            if (finalProperties.Count < RecommendationCount)
            {
                var fallbackProperties = await _propertyRepo.GetTopViewedPublicPropertyCardsAsync(
                    RecommendationCount - finalProperties.Count,
                    finalProperties.Select(property => property.Id).ToList(),
                    userId.Value);

                fallbackPropertiesAdded = fallbackProperties.Count;
                finalProperties.AddRange(fallbackProperties);
            }

            if (!externalResult.Success || recommendedIds.Count == 0)
            {
                _logger.LogWarning(
                    "Recommendations failed entirely for user {UserId}. Reason: {Reason}. Returned {FallbackCount} top-viewed properties instead.",
                    userId.Value,
                    externalResult.FailureReason ?? "No recommendation ids were returned.",
                    finalProperties.Count);
            }
            else if (fallbackPropertiesAdded > 0)
            {
                _logger.LogInformation(
                    "Recommendations partially succeeded for user {UserId}. Accepted {RecommendedCount} external properties and filled {FallbackCount} with top-viewed properties.",
                    userId.Value,
                    recommendedProperties.Count,
                    fallbackPropertiesAdded);
            }
            else
            {
                _logger.LogInformation(
                    "Recommendations succeeded for user {UserId}. Returned {RecommendedCount} external properties without fallback.",
                    userId.Value,
                    finalProperties.Count);
            }

            return ServiceResult<PropertySearchResultDto>.Ok(new PropertySearchResultDto
            {
                Items = finalProperties,
                TotalCount = finalProperties.Count,
                Page = 1,
                PageSize = RecommendationCount
            });
        }

        private static List<long> DeduplicatePreservingOrder(IEnumerable<long> propertyIds)
        {
            var seen = new HashSet<long>();
            var results = new List<long>();

            foreach (var propertyId in propertyIds)
            {
                if (seen.Add(propertyId))
                {
                    results.Add(propertyId);
                }
            }

            return results;
        }
    }
}
