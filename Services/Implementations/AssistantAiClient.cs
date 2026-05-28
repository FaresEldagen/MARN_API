using System.Text;
using System.Text.Json;
using MARN_API.Configurations;
using MARN_API.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace MARN_API.Services.Implementations
{
    public class AssistantAiClient : IAssistantAiClient
    {
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

        public async Task<string> GetAssistantResponseAsync(Guid sessionId, CancellationToken cancellationToken = default)
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

            var content = ExtractAssistantContent(responseBody);
            if (string.IsNullOrWhiteSpace(content))
                throw new InvalidOperationException("Assistant AI response content was empty.");

            return content;
        }

        private static StringContent CreateJsonContent<T>(T payload)
        {
            return new StringContent(
                JsonSerializer.Serialize(payload, JsonOptions),
                Encoding.UTF8,
                "application/json");
        }

        private static string ExtractAssistantContent(string responseBody)
        {
            if (string.IsNullOrWhiteSpace(responseBody))
                return string.Empty;

            try
            {
                using var document = JsonDocument.Parse(responseBody);
                var root = document.RootElement;

                if (root.ValueKind == JsonValueKind.String)
                    return root.GetString() ?? string.Empty;

                if (root.ValueKind == JsonValueKind.Object)
                {
                    foreach (var propertyName in new[] { "content", "message", "response" })
                    {
                        if (root.TryGetProperty(propertyName, out var property) &&
                            property.ValueKind == JsonValueKind.String)
                        {
                            return property.GetString() ?? string.Empty;
                        }
                    }
                }
            }
            catch (JsonException)
            {
                return responseBody;
            }

            return responseBody;
        }
    }
}
