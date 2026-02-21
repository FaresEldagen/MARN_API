using MARN_API.DTOs;
using MARN_API.Models;
using Microsoft.AspNetCore.Identity;

namespace MARN_API.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<ApplicationUser?> GetUserByIdAsync(long id);
        public Task<ApplicationUser?> GetUserByEmailAsync(string email);
        public Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        public Task<ICollection<string>> GetUserRolesAsync(ApplicationUser user);
        public Task<IdentityResult> UpdateUserAsync(UpdateUserDto updateUserDto);
        public Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        public Task<IdentityResult> DeleteUserAsync(long id);
        public Task<IdentityResult> RegisterUserAsync(RegisterDto model);
        //Task<IdentityResult> ConfirmEmailAsync(Guid userId, string token);
        Task ResendEmailConfirmationAsync(string email);
        //Task<bool> CheckEmailTakenAlready(string email);
    }
}
