using MARN_API.Models;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MARN_API.Services.Implementations
{
    public class OwnerService : IOwnerService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccountService _accountService;

        public OwnerService(UserManager<ApplicationUser> userManager, IAccountService accountService)
        {
            _userManager = userManager;
            _accountService = accountService;
        }


        public async Task<ServiceResult<string>> AddOwnerRole(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return ServiceResult<string>.Fail("User not found.");
            }

            if (!await _userManager.IsInRoleAsync(user, "Owner"))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "Owner");
                if (!roleResult.Succeeded)
                {
                    return ServiceResult<string>.Fail("Failed to add owner role.", roleResult.Errors.Select(e => e.Description).ToList());
                }
            }

            var loginResponse = await _accountService.CreateJwtForUserAsync(user);

            return ServiceResult<string>.Ok(
                loginResponse.Token,
                "Successfully became an owner",
                code: "ZZ_OWNER_ROLE_ADDED_SUCCESSFULLY");
        }
    }
}
