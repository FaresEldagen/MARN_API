using MARN_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace MARN_API.Attributes
{
    public class CheckRoleAttribute : TypeFilterAttribute
    {
        public CheckRoleAttribute(string roles) : base(typeof(CheckRoleFilter))
        {
            Arguments = new object[] { roles };
        }
    }

    public class CheckRoleFilter : IAsyncAuthorizationFilter
    {
        private readonly string[] _roles;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckRoleFilter(string roles, UserManager<ApplicationUser> userManager)
        {
            _roles = roles.Split(',').Select(r => r.Trim()).ToArray();
            _userManager = userManager;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            bool hasRole = false;
            foreach (var role in _roles)
            {
                if (await _userManager.IsInRoleAsync(user, role))
                {
                    hasRole = true;
                    break;
                }
            }

            if (!hasRole)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
