using System.Text;
using System.Text.Json;
using MARN_API.Configurations;
using MARN_API.DTOs.Assistant;
using MARN_API.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace MARN_API.Services.Implementations
{
    public class AssistantAiClient : IAssistantAiClient
    {
        private const int MaxImagePaths = 20;
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
        private readonly HttpClient _httpClient;
        private readonly AssistantAiOptions _options;
        private readonly ILogger<AssistantAiClient> _logger;

        public AssistantAiClient(
            HttpClient httpClient,
            IOptions<AssistantAiOptions> options,
            ILogger<AssistantAiClient> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<AssistantAiResponse> GetAssistantResponseAsync(Guid sessionId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_options.ChatUrl))
                throw new InvalidOperationException("Assistant AI chat URL is not configured.");

            using var request = new HttpRequestMessage(HttpMethod.Post, _options.ChatUrl)
            {
                Content = CreateJsonContent(new { sessionId })
            };

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Assistant AI request failed for session {SessionId}. Status: {StatusCode}. Body: {ResponseBody}",
                    sessionId,
                    (int)response.StatusCode,
                    responseBody);

                throw new HttpRequestException($"Assistant AI request failed with status {(int)response.StatusCode}.");
            }

            var assistantResponse = ExtractAssistantResponse(responseBody);
            if (string.IsNullOrWhiteSpace(assistantResponse.Content))
                throw new InvalidOperationException("Assistant AI response content was empty.");

            return assistantResponse;
        }

        private static StringContent CreateJsonContent<T>(T payload)
        {
            return new StringContent(
                JsonSerializer.Serialize(payload, JsonOptions),
                Encoding.UTF8,
                "application/json");
        }

        private static AssistantAiResponse ExtractAssistantResponse(string responseBody)
        {
            if (string.IsNullOrWhiteSpace(responseBody))
                return new AssistantAiResponse();

            try
            {
                using var document = JsonDocument.Parse(responseBody);
                var root = document.RootElement;

                if (root.ValueKind == JsonValueKind.String)
                    return new AssistantAiResponse { Content = root.GetString() ?? string.Empty };

                if (root.ValueKind == JsonValueKind.Object)
                {
                    var result = new AssistantAiResponse();

                    foreach (var propertyName in new[] { "content", "message", "response" })
                    {
                        if (root.TryGetProperty(propertyName, out var property) &&
                            property.ValueKind == JsonValueKind.String)
                        {
                            result.Content = property.GetString() ?? string.Empty;
                            break;
                        }
                    }

                    if (root.TryGetProperty("imagePaths", out var imagePathsProperty) &&
                        imagePathsProperty.ValueKind == JsonValueKind.Array)
                    {
                        result.ImagePaths = imagePathsProperty
                            .EnumerateArray()
                            .Where(path => path.ValueKind == JsonValueKind.String)
                            .Select(path => path.GetString() ?? string.Empty)
                            .Where(IsValidImagePath)
                            .Select(path => path.Trim())
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .Take(MaxImagePaths)
                            .ToList();
                    }

                    return result;
                }
            }
            catch (JsonException)
            {
                return new AssistantAiResponse { Content = responseBody };
            }

            return new AssistantAiResponse();
        }

        private static bool IsValidImagePath(string? value)
        {
            var path = value?.Trim();
            if (string.IsNullOrWhiteSpace(path))
                return false;

            if (!path.StartsWith('/') || path.StartsWith("//"))
                return false;

            if (path.Contains('\\') || path.Contains("..", StringComparison.Ordinal))
                return false;

            if (Uri.TryCreate(path, UriKind.Absolute, out _))
                return false;

            return true;
        }
    }
}
