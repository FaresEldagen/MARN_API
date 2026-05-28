using System.Net.Http;
using System.Text;
using System.Text.Json;
using MARN_API.Configurations;
using MARN_API.Models;
using MARN_API.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace MARN_API.Services.Implementations
{
    public class ExternalPropertyAiClient : IExternalPropertyAiClient
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
        private readonly HttpClient _httpClient;
        private readonly ExternalPropertyAiOptions _options;
        private readonly ILogger<ExternalPropertyAiClient> _logger;

        public ExternalPropertyAiClient(
            HttpClient httpClient,
            IOptions<ExternalPropertyAiOptions> options,
            ILogger<ExternalPropertyAiClient> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<ExternalRecommendationResult> GetRecommendedPropertyIdsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_options.RecommendationUrl))
            {
                _logger.LogWarning("External recommendation URL is not configured.");
                return ExternalRecommendationResult.Failed("Recommendation URL is not configured.");
            }

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, _options.RecommendationUrl)
                {
                    Content = CreateJsonContent(new { userId })
                };
                using var response = await _httpClient.SendAsync(request, cancellationToken);

                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "External recommendation request failed with status {StatusCode}. Body: {ResponseBody}",
                        (int)response.StatusCode,
                        responseBody);

                    return ExternalRecommendationResult.Failed($"Recommendation request failed with status {(int)response.StatusCode}.");
                }

                List<long>? propertyIds;
                try
                {
                    propertyIds = JsonSerializer.Deserialize<List<long>>(responseBody);
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(
                        ex,
                        "External recommendation response was not a valid property id array. Body: {ResponseBody}",
                        responseBody);

                    return ExternalRecommendationResult.Failed("Recommendation response payload was invalid.");
                }

                if (propertyIds == null)
                {
                    _logger.LogWarning("External recommendation response deserialized to null. Body: {ResponseBody}", responseBody);
                    return ExternalRecommendationResult.Failed("Recommendation response payload was null.");
                }

                _logger.LogInformation(
                    "External recommendation request succeeded for user {UserId}. Returned {Count} property ids. Body: {ResponseBody}",
                    userId,
                    propertyIds.Count,
                    responseBody);

                return ExternalRecommendationResult.Succeeded(propertyIds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "External recommendation request threw an exception for user {UserId}", userId);
                return ExternalRecommendationResult.Failed("Recommendation request failed due to an exception.");
            }
        }

        public Task NotifyPropertyAddedAsync(long propertyId, CancellationToken cancellationToken = default)
        {
            return PostPropertyEventAsync(_options.PropertyAddedUrl, propertyId, "added", cancellationToken);
        }

        public Task NotifyPropertyUpdatedAsync(long propertyId, CancellationToken cancellationToken = default)
        {
            return PostPropertyEventAsync(_options.PropertyUpdatedUrl, propertyId, "updated", cancellationToken);
        }

        public Task NotifyPropertyDeletedAsync(long propertyId, CancellationToken cancellationToken = default)
        {
            return PostPropertyEventAsync(_options.PropertyDeletedUrl, propertyId, "deleted", cancellationToken);
        }

        private async Task PostPropertyEventAsync(string? url, long propertyId, string eventName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                _logger.LogWarning("External property AI {EventName} URL is not configured for property {PropertyId}", eventName, propertyId);
                return;
            }

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = CreateJsonContent(new { propertyId })
                };
                using var response = await _httpClient.SendAsync(request, cancellationToken);

                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation(
                        "External property AI {EventName} request succeeded for property {PropertyId}. Status: {StatusCode}. Body: {ResponseBody}",
                        eventName,
                        propertyId,
                        (int)response.StatusCode,
                        responseBody);

                    return;
                }

                _logger.LogWarning(
                    "External property AI {EventName} request failed for property {PropertyId}. Status: {StatusCode}. Body: {ResponseBody}",
                    eventName,
                    propertyId,
                    (int)response.StatusCode,
                    responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "External property AI {EventName} request threw an exception for property {PropertyId}", eventName, propertyId);
            }
        }

        private static StringContent CreateJsonContent<T>(T payload)
        {
            return new StringContent(
                JsonSerializer.Serialize(payload, JsonOptions),
                Encoding.UTF8,
                "application/json");
        }
    }
}
