using MARN_API.Models;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MARN_API.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<IdentityResult> AddAdminRole(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            return await _userManager.AddToRoleAsync(user, "Admin");
        }

        public async Task<IdentityResult> RemoveAdminRole(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            return await _userManager.RemoveFromRoleAsync(user, "Admin");
        }

        public async Task<ICollection<Admin>> GetAllAdmins()
        {
            var Admins = await _userManager.GetUsersInRoleAsync("Admin");
            return Admins.Cast<Admin>().ToList();
        }
    }
}
