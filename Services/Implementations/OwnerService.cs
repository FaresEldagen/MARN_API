using MARN_API.Data;
using MARN_API.Models;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Services.Implementations
{
    public class OwnerService : IOwnerService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;

        public OwnerService(UserManager<ApplicationUser> userManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<IdentityResult> AddOwnerRole(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            if (!await _userManager.IsInRoleAsync(user, "Owner"))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "Owner");
                if (!roleResult.Succeeded)
                {
                    return roleResult;
                }
            }

            await _dbContext.Database.ExecuteSqlInterpolatedAsync(
                $"UPDATE AspNetUsers SET Discriminator = {"Owner"} WHERE Id = {id}");

            return IdentityResult.Success;
        }
    }
}
