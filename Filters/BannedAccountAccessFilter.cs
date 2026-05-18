using System.Security.Claims;
using MARN_API.Attributes;
using MARN_API.Data;
using MARN_API.Enums.Account;
using MARN_API.Localization;
using MARN_API.Models;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Filters
{
    public class BannedAccountAccessFilter : IAsyncActionFilter
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BannedAccountAccessFilter> _logger;

        public BannedAccountAccessFilter(AppDbContext context, ILogger<BannedAccountAccessFilter> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (CanBypassFilter(context))
            {
                await next();
                return;
            }

            var userIdValue = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdValue) || !Guid.TryParse(userIdValue, out var userId))
            {
                await next();
                return;
            }

            var user = await _context.Users
                .IgnoreQueryFilters()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user?.AccountStatus != AccountStatus.Banned)
            {
                await next();
                return;
            }

            _logger.LogWarning("Blocked banned user {UserId} from accessing {Path}", userId, context.HttpContext.Request.Path);
            var localizer = context.HttpContext.RequestServices.GetRequiredService<IAppTextLocalizer>();
            var code = "ACCOUNT_BANNED_ACCESS_DENIED";
            var message = "Banned accounts cannot access this endpoint.";

            context.Result = new ObjectResult(new ErrorResponse
            {
                Code = code,
                Message = localizer.LocalizeMessage(code, message),
                StatusCode = StatusCodes.Status403Forbidden,
                Path = context.HttpContext.Request.Path,
                TraceId = context.HttpContext.TraceIdentifier,
                Timestamp = DateTime.UtcNow
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }

        private static bool CanBypassFilter(ActionExecutingContext context)
        {
            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint == null)
                return true;

            if (endpoint.Metadata.GetMetadata<IAllowAnonymous>() != null)
                return true;

            if (endpoint.Metadata.GetMetadata<DisallowBannedUserAttribute>() == null)
                return true;

            return false;
        }
    }
}
