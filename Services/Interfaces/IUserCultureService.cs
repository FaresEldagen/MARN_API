using System.Globalization;

namespace MARN_API.Services.Interfaces
{
    public interface IUserCultureService
    {
        Task<CultureInfo> ResolveUserCultureAsync(string? userId);
    }
}
