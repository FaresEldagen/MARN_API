using Google.Apis.Auth;
using MARN_API.DTOs.Auth;
using MARN_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MARN_API.Services.Interfaces
{
    public interface IAccountService
    {
        #region Login And 2FA
        public Task<ServiceResult<LoginResponseDto>> LoginAsync(LogInDto dto);
        public Task<ServiceResult<LoginResponseDto>> GoogleLoginAsync(GoogleLoginDto dto);
        public Task<ServiceResult<LoginResponseDto>> VerifyTwoFactorAsync(VerifyTwoFactorDto dto);
        public Task<LoginResponseDto> CreateJwtForUserAsync(ApplicationUser user, bool rememberMe = false, string provider = null!);
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
    }
}
