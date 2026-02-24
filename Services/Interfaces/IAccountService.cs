using Google.Apis.Auth;
using MARN_API.DTOs;
using MARN_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public Task<IdentityResult> ConfirmEmailAsync(Guid userId, string token);
        public Task<IdentityResult> RegisterUserAsync(RegisterDto model);
        // public  Task<ActionResult<TokenDto>> Login(LogInDto logInDto);
        Task ResendEmailConfirmationAsync(string email);

        Task<ServiceResult<bool>> ForgotPasswordAsync(ForgotPasswordRequestDto request);

        Task<ServiceResult<bool>> ValidateResetTokenAsync(ValidateResetTokenRequestDto request);

        Task<ServiceResult<bool>> ResetPasswordAsync(ResetPasswordRequestDto request);

        //  Task<ServiceResult<LoginResponseDto>> VerifyTwoFactorAsync(VerifyTwoFactorDto dto);

        Task<ServiceResult<LoginResponseDto>> LoginAsync(LogInDto dto);
        Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user);
        Task<string> GenerateTwoFactorTokenAsync(ApplicationUser user);
        Task<ServiceResult<LoginResponseDto>> VerifyTwoFactorAsync(VerifyTwoFactorDto dto);
        Task<ServiceResult<bool>> ToggleTwoFactorAsync(string userId, string? password = null);
        public Task<GoogleJsonWebSignature.Payload?> ValidateGoogleTokenAsync(string idToken);
        public  Task<ServiceResult<LoginResponseDto>> GoogleLoginAsync(GoogleLoginDto dto);
    }
}
