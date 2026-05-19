using Hangfire.Dashboard;

namespace MARN_API.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Allow local access
            var isLocal = httpContext.Connection.RemoteIpAddress?.ToString() == "127.0.0.1" || 
                          httpContext.Connection.RemoteIpAddress?.ToString() == "::1" ||
                          httpContext.Connection.RemoteIpAddress?.Equals(httpContext.Connection.LocalIpAddress) == true;

            if (isLocal)
            {
                return true;
            }

            // For remote access:
            // Since this API uses JWT Authentication (Bearer tokens) and the Hangfire Dashboard 
            // is a browser-based UI that does not easily send Authorization headers, 
            // you'll need to implement an alternative approach for remote access.
            // A common approach is to use the 'Hangfire.Dashboard.Basic.Authentication' NuGet package.
            
            // Or if using cookies:
            // return httpContext.User.Identity?.IsAuthenticated == true && httpContext.User.IsInRole("Admin");

            return false;
        }
    }
}
