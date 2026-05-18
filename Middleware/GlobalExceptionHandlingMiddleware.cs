using MARN_API.Models;
using MARN_API.Services.Interfaces;
using System.Net;
using System.Text.Json;
using MARN_API.Localization;

namespace MARN_API.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred. Request Path: {Path}", context.Request.Path);
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started for path {Path}. The global exception handler cannot rewrite the response body.", context.Request.Path);
                    throw;
                }

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var localizer = context.RequestServices.GetRequiredService<IAppTextLocalizer>();
            var statusCode = HttpStatusCode.InternalServerError;
            var code = "INTERNAL_ERROR";
            var message = "An unexpected error occurred. Please try again later.";
            var details = (string?)null;

            // Handle different exception types
            switch (exception)
            {
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    code = "ACCESS_FORBIDDEN";
                    message = "You are not authorized to perform this action.";
                    break;

                case ArgumentException argEx:
                    statusCode = HttpStatusCode.BadRequest;
                    message = argEx.Message;
                    break;

                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = string.IsNullOrWhiteSpace(exception.Message) || exception.Message.StartsWith("The given key", StringComparison.Ordinal)
                        ? "The requested resource was not found."
                        : exception.Message;
                    break;

                case InvalidOperationException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;

                case TimeoutException:
                    statusCode = HttpStatusCode.RequestTimeout;
                    code = "REQUEST_TIMEOUT";
                    message = "The request timed out. Please try again.";
                    break;

                default:
                    // For production, don't expose internal error details
                    // In development, include exception details
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                    {
                        details = exception.ToString();
                    }
                    break;
            }

            code = LocalizationKeyBuilder.BuildErrorCode(code == "INTERNAL_ERROR" || code == "ACCESS_FORBIDDEN" || code == "REQUEST_TIMEOUT" ? code : null, message, null, code);

            var errorResponse = new ErrorResponse
            {
                Code = code,
                Message = localizer.LocalizeMessage(code, message),
                Details = details,
                StatusCode = (int)statusCode,
                Path = context.Request.Path,
                TraceId = context.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            };

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(errorResponse, options);
            return context.Response.WriteAsync(json);
        }
    }
}

