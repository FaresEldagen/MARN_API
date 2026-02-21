using MARN_API.Models;
using Microsoft.AspNetCore.Identity;

namespace MARN_API.Services.Interfaces
{
    public interface IAdminService
    {
        public Task<IdentityResult> AddAdminRole(Guid id);
        public Task<IdentityResult> RemoveAdminRole(Guid id);
        public Task<ICollection<Admin>> GetAllAdmins();
    }
}
