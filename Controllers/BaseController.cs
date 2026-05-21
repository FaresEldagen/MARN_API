using MARN_API.DTOs.Common;
using MARN_API.Enums;
using MARN_API.Localization;
using MARN_API.Models;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MARN_API.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected ActionResult HandleServiceResult<T>(ServiceResult<T> result)
        {
            var code = result.Success
                ? ResolveSuccessCode(result)
                : LocalizationKeyBuilder.BuildErrorCode(result.Code, result.Message, result.Errors, GetDefaultCode(result.ResultType));
            var primaryMessage = result.Success
                ? result.Message
                : LocalizationKeyBuilder.ResolvePrimaryMessage(result.Message, result.Errors, "An error occurred.");
            var message = Localizer.LocalizeMessage(code, primaryMessage, arguments: result.MessageArguments);
            ResponsePayloadLocalizer.Localize(result.Data);

            return result.ResultType switch
            {
                ServiceResultType.Success => Ok(new ApiResponseDto<T> { Code = code, Message = message, Data = result.Data }),
                ServiceResultType.Created => StatusCode(201, new ApiResponseDto<T> { Code = code, Message = message, Data = result.Data }),
                ServiceResultType.RequiresTwoFactor => Accepted(new ApiResponseDto<T> { Code = code, Message = message, Data = result.Data }),
                ServiceResultType.Unauthorized => Unauthorized(CreateErrorResponse(StatusCodes.Status401Unauthorized, result.Message, result.Errors, result.Action, code, result.MessageArguments)),
                ServiceResultType.NotFound => NotFound(CreateErrorResponse(StatusCodes.Status404NotFound, result.Message, result.Errors, result.Action, code, result.MessageArguments)),
                ServiceResultType.Forbidden => StatusCode(StatusCodes.Status403Forbidden, CreateErrorResponse(StatusCodes.Status403Forbidden, result.Message, result.Errors, result.Action, code, result.MessageArguments)),
                ServiceResultType.Conflict => Conflict(CreateErrorResponse(StatusCodes.Status409Conflict, result.Message, result.Errors, result.Action, code, result.MessageArguments)),
                _ => BadRequest(CreateErrorResponse(StatusCodes.Status400BadRequest, result.Message, result.Errors, result.Action, code, result.MessageArguments))
            };
        }

        protected bool TryGetUserId(out Guid userId)
        {
            userId = Guid.Empty;
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return !string.IsNullOrEmpty(claim) && Guid.TryParse(claim, out userId);
        }

        protected ErrorResponse CreateErrorResponse(
            int statusCode,
            string? message,
            List<string>? errors = null,
            string? action = null,
            string? code = null,
            object?[]? messageArguments = null)
        {
            var primaryMessage = LocalizationKeyBuilder.ResolvePrimaryMessage(message, errors, "An error occurred.");
            var resolvedCode = LocalizationKeyBuilder.BuildErrorCode(code, primaryMessage, errors, GetDefaultCode(statusCode));

            return new ErrorResponse
            {
                Code = resolvedCode,
                Message = Localizer.LocalizeMessage(resolvedCode, primaryMessage, arguments: messageArguments),
                Action = action,
                StatusCode = statusCode,
                Path = HttpContext.Request.Path,
                TraceId = HttpContext.TraceIdentifier,
                Timestamp = DateTime.UtcNow,
                Errors = errors == null ? null : new Dictionary<string, string[]>
                {
                    ["general"] = errors.Select(error => Localizer.LocalizeMessage(null, error)).ToArray()
                }
            };
        }

        protected UnauthorizedObjectResult UnauthorizedLocalized(string message, string? code = null)
            => Unauthorized(CreateErrorResponse(StatusCodes.Status401Unauthorized, message, code: code));

        protected BadRequestObjectResult BadRequestLocalized(string message, string? code = null)
            => BadRequest(CreateErrorResponse(StatusCodes.Status400BadRequest, message, code: code));

        protected UnauthorizedObjectResult UnauthorizedUserIdMissing()
            => UnauthorizedLocalized("User ID not found in token.", "AUTH_USER_ID_MISSING");

        protected UnauthorizedObjectResult UnauthorizedUserIdMismatch()
            => UnauthorizedLocalized("User ID mismatch.", "AUTH_USER_ID_MISMATCH");

        protected BadRequestObjectResult BadRequestUserIdAndTokenRequired()
            => BadRequestLocalized("User ID and token are required.", "USER_ID_AND_TOKEN_REQUIRED");

        protected BadRequestObjectResult BadRequestTokenRequired()
            => BadRequestLocalized("Token is required.", "TOKEN_REQUIRED");

        protected BadRequestObjectResult BadRequestEmptySearchQuery()
            => BadRequestLocalized("Empty search query.", "EMPTY_SEARCH_QUERY");

        protected BadRequestObjectResult BadRequestOtherUserIdRequired()
            => BadRequestLocalized("Other User ID is required.", "OTHER_USER_ID_REQUIRED");

        protected BadRequestObjectResult BadRequestInvalidPagination()
            => BadRequestLocalized("pageNumber and pageSize must be greater than 0.", "INVALID_PAGINATION");

        protected BadRequestObjectResult BadRequestWebhookSignatureInvalid()
            => BadRequestLocalized("Webhook signature validation failed.", "INVALID_WEBHOOK_SIGNATURE");

        private IAppTextLocalizer Localizer => HttpContext.RequestServices.GetRequiredService<IAppTextLocalizer>();
        private IResponsePayloadLocalizer ResponsePayloadLocalizer => HttpContext.RequestServices.GetRequiredService<IResponsePayloadLocalizer>();

        private static string GetDefaultCode(ServiceResultType resultType)
        {
            return resultType switch
            {
                ServiceResultType.Created => "CREATED",
                ServiceResultType.RequiresTwoFactor => "REQUIRES_TWO_FACTOR",
                ServiceResultType.Unauthorized => "UNAUTHORIZED",
                ServiceResultType.NotFound => "NOT_FOUND",
                ServiceResultType.Forbidden => "FORBIDDEN",
                ServiceResultType.Conflict => "CONFLICT",
                ServiceResultType.BadRequest => "BAD_REQUEST",
                ServiceResultType.InternalError => "INTERNAL_ERROR",
                _ => "SUCCESS"
            };
        }

        private static string ResolveSuccessCode<T>(ServiceResult<T> result)
        {
            if (!string.IsNullOrWhiteSpace(result.Code))
            {
                return result.Code.Trim();
            }

            if (!string.IsNullOrWhiteSpace(result.Message))
            {
                return LocalizationKeyBuilder.BuildLiteralKey(result.Message);
            }

            return GetDefaultCode(result.ResultType);
        }

        private static string GetDefaultCode(int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.Status400BadRequest => "BAD_REQUEST",
                StatusCodes.Status401Unauthorized => "UNAUTHORIZED",
                StatusCodes.Status403Forbidden => "FORBIDDEN",
                StatusCodes.Status404NotFound => "NOT_FOUND",
                StatusCodes.Status409Conflict => "CONFLICT",
                StatusCodes.Status429TooManyRequests => "RATE_LIMIT_EXCEEDED",
                _ => "INTERNAL_ERROR"
            };
        }
    }
}
