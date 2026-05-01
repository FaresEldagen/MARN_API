using MARN_API.DTOs.Common;
using MARN_API.Enums;
using MARN_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MARN_API.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected ActionResult HandleServiceResult<T>(ServiceResult<T> result)
        {
            return result.ResultType switch
            {
                ServiceResultType.Success => Ok(new ApiResponseDto<T> { Message = result.Message, Data = result.Data }),
                ServiceResultType.Created => StatusCode(201, new ApiResponseDto<T> { Message = result.Message, Data = result.Data }),
                ServiceResultType.RequiresTwoFactor => Accepted(new ApiResponseDto<T> { Message = result.Message, Data = result.Data }),
                ServiceResultType.Unauthorized => Unauthorized(CreateErrorResponse(StatusCodes.Status401Unauthorized, result.Message, result.Errors, result.Action)),
                ServiceResultType.NotFound => NotFound(CreateErrorResponse(StatusCodes.Status404NotFound, result.Message)),
                ServiceResultType.Forbidden => StatusCode(StatusCodes.Status403Forbidden, CreateErrorResponse(StatusCodes.Status403Forbidden, result.Message)),
                ServiceResultType.Conflict => Conflict(CreateErrorResponse(StatusCodes.Status409Conflict, result.Message, result.Errors)),
                _ => BadRequest(CreateErrorResponse(StatusCodes.Status400BadRequest, result.Message, result.Errors))
            };
        }

        protected bool TryGetUserId(out Guid userId)
        {
            userId = Guid.Empty;
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return !string.IsNullOrEmpty(claim) && Guid.TryParse(claim, out userId);
        }

        protected ErrorResponse CreateErrorResponse(int statusCode, string? message, List<string>? errors = null, string? action = null)
        {
            return new ErrorResponse
            {
                Message = message ?? "An error occurred.",
                Action = action,
                StatusCode = statusCode,
                Path = HttpContext.Request.Path,
                TraceId = HttpContext.TraceIdentifier,
                Timestamp = DateTime.UtcNow,
                Errors = errors == null ? null : new Dictionary<string, string[]>
                {
                    ["general"] = errors.ToArray()
                }
            };
        }
    }
}
