using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MARN_API.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            // ASP.NET Core Identity stores the User's ID in the NameIdentifier claim.
            // When migrating to React (JWT), ensure the JWT also includes the NameIdentifier claim.
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
