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
                ServiceResultType.Success => Ok(new { message = result.Message, data = result.Data }),
                ServiceResultType.Created => StatusCode(201, new { message = result.Message, data = result.Data }),
                ServiceResultType.RequiresTwoFactor => Accepted(new { message = result.Message, data = result.Data }),
                ServiceResultType.Unauthorized => Unauthorized(new { message = result.Message }),
                ServiceResultType.NotFound => NotFound(new { message = result.Message }),
                ServiceResultType.Forbidden => StatusCode(403, new { message = result.Message }),
                ServiceResultType.Conflict => Conflict(new { message = result.Message, errors = result.Errors }),
                _ => BadRequest(new { message = result.Message, errors = result.Errors })
            };
        }

        protected bool TryGetUserId(out Guid userId)
        {
            userId = Guid.Empty;
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return !string.IsNullOrEmpty(claim) && Guid.TryParse(claim, out userId);
        }
    }
}