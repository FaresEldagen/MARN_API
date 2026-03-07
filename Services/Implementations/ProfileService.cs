using AutoMapper;
using MARN_API.Models;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MARN_API.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountService> _logger;
        private readonly IMapper _mapper;
        public ProfileService(
            UserManager<ApplicationUser> userManager,
            IEmailService emailService,
            IConfiguration configuration,
            IMapper mapper,
            ITokenService tokenService,
            ILogger<AccountService> logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResult<bool>> ToggleTwoFactorAsync(string userId, string? password = null)
        {
            _logger.LogInformation("Toggle2FA attempt for userId: {userId}", userId);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Toggle2FA failed: User not found for userId: {userId}", userId);
                return ServiceResult<bool>.Fail("User not found");
            }

            // Optional: verify password before allowing toggle
            if (!user.TwoFactorEnabled)
            {
                if (string.IsNullOrEmpty(password))
                {
                    _logger.LogWarning("Toggle2FA failed: Invalid password for user: {UserId}", user.Id);
                    return ServiceResult<bool>.Fail("Invalid password");
                }
                else
                {
                    bool CheckPassword = await _userManager.CheckPasswordAsync(user, password);
                    if (!CheckPassword)
                    {
                        _logger.LogWarning("Toggle2FA failed: Invalid password for user: {UserId}", user.Id);
                        return ServiceResult<bool>.Fail("Invalid password");
                    }
                }

            }

            bool newState = !user.TwoFactorEnabled;

            var result = await _userManager.SetTwoFactorEnabledAsync(user, newState);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Toggle2FA failed: Failed to Update the database for user: {UserId}", user.Id);
                return ServiceResult<bool>.Fail("Failed to toggle 2FA", result.Errors.Select(e => e.Description).ToList());
            }

            _logger.LogInformation("User {UserId} toggled 2FA. Enabled={Enabled}", user.Id, newState);
            return ServiceResult<bool>.Ok(newState, $"Two-Factor Authentication is now {(newState ? "enabled" : "disabled")}");
        }

        //public async Task<IdentityResult> UpdateUserAsync(UpdateUserDto updateUserDto)
        //{
        //    var user = await _userManager.FindByIdAsync(updateUserDto.id.ToString());
        //    if (user == null)
        //        return IdentityResult.Failed(new IdentityError { Description = "User not found." });

        //    user = _mapper.Map(updateUserDto, user);

        //    return await _userManager.UpdateAsync(user);
        //}

        //public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        //{
        //    var user = await _userManager.FindByIdAsync(changePasswordDto.id.ToString());
        //    if (user == null)
        //        return IdentityResult.Failed(new IdentityError { Description = "User not found." });

        //    if (!await _userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword))
        //        return IdentityResult.Failed(new IdentityError { Description = "Current password is incorrect." });

        //    return await _userManager.ChangePasswordAsync(
        //        user,
        //        changePasswordDto.CurrentPassword,
        //        changePasswordDto.NewPassword);
        //}

        //public async Task<IdentityResult> DeleteUserAsync(long id)
        //{
        //    var user = await _userManager.FindByIdAsync(id.ToString());
        //    if (user == null)
        //        return IdentityResult.Failed(new IdentityError { Description = "User not found." });

        //    user.DeletedAt = DateTime.UtcNow;

        //    return await _userManager.UpdateAsync(user);
        //}
    }
}
