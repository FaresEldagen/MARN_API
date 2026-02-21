using System.Diagnostics;
using System.Text;

namespace MARN_API.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly HashSet<string> _sensitiveHeaders = new()
        {
            "Authorization",
            "Cookie",
            "X-API-Key"
        };

        private readonly HashSet<string> _sensitiveBodyFields = new(StringComparer.OrdinalIgnoreCase)
        {
            "password",
            "token",
            "secret",
            "apikey",
            "authorization"
        };

        public RequestLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestPath = context.Request.Path;
            var requestMethod = context.Request.Method;

            // Log request
            await LogRequestAsync(context);

            // Capture original response body stream
            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();

                // Log response
                await LogResponseAsync(context, stopwatch.ElapsedMilliseconds);

                // Copy response back to original stream
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task LogRequestAsync(HttpContext context)
        {
            var request = context.Request;
            var requestPath = request.Path + request.QueryString;

            // Log basic request info
            _logger.LogInformation(
                "HTTP {Method} {Path} requested from {RemoteIp}",
                request.Method,
                requestPath,
                context.Connection.RemoteIpAddress);

            // Log headers (sanitized)
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                var headers = request.Headers
                    .Where(h => !_sensitiveHeaders.Contains(h.Key))
                    .ToDictionary(h => h.Key, h => h.Value.ToString());

                _logger.LogDebug("Request Headers: {@Headers}", headers);
            }

            // Log request body for non-sensitive endpoints (if enabled)
            if (_logger.IsEnabled(LogLevel.Debug) && 
                request.ContentLength > 0 && 
                request.ContentLength < 10240 && // Only log if body is less than 10KB
                request.ContentType?.Contains("application/json") == true)
            {
                request.EnableBuffering();
                var body = await new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true).ReadToEndAsync();
                request.Body.Position = 0;

                var sanitizedBody = SanitizeJsonBody(body);
                _logger.LogDebug("Request Body: {Body}", sanitizedBody);
            }
        }

        private async Task LogResponseAsync(HttpContext context, long elapsedMilliseconds)
        {
            var response = context.Response;
            var requestPath = context.Request.Path + context.Request.QueryString;

            // Read response body
            response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(response.Body, Encoding.UTF8).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            var sanitizedResponseBody = SanitizeJsonBody(responseBody);

            _logger.LogInformation(
                "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs}ms. Response Size: {Size} bytes",
                context.Request.Method,
                requestPath,
                response.StatusCode,
                elapsedMilliseconds,
                responseBody.Length);

            if (_logger.IsEnabled(LogLevel.Debug) && !string.IsNullOrEmpty(sanitizedResponseBody))
            {
                _logger.LogDebug("Response Body: {Body}", sanitizedResponseBody);
            }
        }

        private string SanitizeJsonBody(string jsonBody)
        {
            if (string.IsNullOrWhiteSpace(jsonBody))
                return jsonBody;

            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(jsonBody);
                var sanitized = SanitizeJsonElement(doc.RootElement);
                return System.Text.Json.JsonSerializer.Serialize(sanitized, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = false
                });
            }
            catch
            {
                // If JSON parsing fails, return original (might not be JSON)
                return jsonBody;
            }
        }

        private object? SanitizeJsonElement(System.Text.Json.JsonElement element)
        {
            switch (element.ValueKind)
            {
                case System.Text.Json.JsonValueKind.Object:
                    var obj = new Dictionary<string, object?>();
                    foreach (var prop in element.EnumerateObject())
                    {
                        var key = prop.Name;
                        var value = prop.Value;

                        if (_sensitiveBodyFields.Contains(key))
                        {
                            obj[key] = "***REDACTED***";
                        }
                        else if (value.ValueKind == System.Text.Json.JsonValueKind.Object || 
                                 value.ValueKind == System.Text.Json.JsonValueKind.Array)
                        {
                            obj[key] = SanitizeJsonElement(value);
                        }
                        else
                        {
                            obj[key] = value.GetRawText();
                        }
                    }
                    return obj;

                case System.Text.Json.JsonValueKind.Array:
                    return element.EnumerateArray()
                        .Select(SanitizeJsonElement)
                        .ToArray();

                default:
                    return element.GetRawText();
            }
        }
    }
}

