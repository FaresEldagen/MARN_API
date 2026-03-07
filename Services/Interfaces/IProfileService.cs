using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IProfileService
    {
        public Task<ServiceResult<bool>> ToggleTwoFactorAsync(string userId, string? password = null);
        //public Task<IdentityResult> UpdateUserAsync(UpdateUserDto updateUserDto);
        //public Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        //public Task<IdentityResult> DeleteUserAsync(long id);
    }
}
