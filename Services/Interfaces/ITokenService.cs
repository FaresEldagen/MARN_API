using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user, ICollection<string> roles,  DateTime expiration,bool includeMfaClaim = false);
    }
}