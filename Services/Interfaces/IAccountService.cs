using Google.Apis.Auth;
using MARN_API.DTOs;
using MARN_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MARN_API.Services.Interfaces
{
    public interface IAccountService
    {
        #region Login And 2FA
        public Task<ServiceResult<LoginResponseDto>> LoginAsync(LogInDto dto);
        public  Task<ServiceResult<LoginResponseDto>> GoogleLoginAsync(GoogleLoginDto dto);
        public Task<ServiceResult<LoginResponseDto>> VerifyTwoFactorAsync(VerifyTwoFactorDto dto);
        #endregion


        #region Register And Confirm Email
        public Task<ServiceResult<bool>> RegisterUserAsync(RegisterDto model);
        public Task<ServiceResult<bool>> ConfirmEmailAsync(Guid userId, string token);
        public Task<ServiceResult<bool>> ResendEmailConfirmationAsync(ResendConfirmationEmailDto request);
        #endregion


        #region Reset Password
        public Task<ServiceResult<bool>> ForgotPasswordAsync(ForgotPasswordRequestDto request);
        public Task<ServiceResult<bool>> ValidateResetTokenAsync(ValidateResetTokenRequestDto request);
        public Task<ServiceResult<bool>> ResetPasswordAsync(ResetPasswordRequestDto request);
        #endregion


        #region Others
        public Task<ServiceResult<bool>> ToggleTwoFactorAsync(string userId, string? password = null);
        //public Task<IdentityResult> UpdateUserAsync(UpdateUserDto updateUserDto);
        //public Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        //public Task<IdentityResult> DeleteUserAsync(long id);
        #endregion
    }
}
