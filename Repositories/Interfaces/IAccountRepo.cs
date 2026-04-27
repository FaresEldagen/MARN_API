using Google.Apis.Auth;
using MARN_API.DTOs.Dashboard;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IAccountRepo
    {
        public Task<ApplicationUser?> GetGoogleUser(string key);
    }
}
