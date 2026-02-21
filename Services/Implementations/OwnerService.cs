using MARN_API.Models;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MARN_API.Services.Implementations
{
    public class OwnerService : IOwnerService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public OwnerService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<IdentityResult> AddOwnerRole(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            return await _userManager.AddToRoleAsync(user, "Owner");
        }
    }
}
