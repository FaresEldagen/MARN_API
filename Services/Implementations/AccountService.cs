using AutoMapper;
using MARN_API.DTOs;
using MARN_API.Models;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Text;

namespace MARN_API.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AccountService(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            IEmailService emailService, 
            IConfiguration configuration, 
            IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _configuration = configuration;
            _mapper = mapper;
        }


        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(long id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<ICollection<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            var user = await _userManager.FindByIdAsync(updateUserDto.id.ToString());
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            user = _mapper.Map(updateUserDto, user);

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(changePasswordDto.id.ToString());
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            if(!await _userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword))
                return IdentityResult.Failed(new IdentityError { Description = "Current password is incorrect." });

            return await _userManager.ChangePasswordAsync(
                user,
                changePasswordDto.CurrentPassword,
                changePasswordDto.NewPassword);
        }

        public async Task<IdentityResult> DeleteUserAsync(long id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            user.DeletedAt = DateTime.UtcNow;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDto model)
        {
            var user = _mapper.Map<ApplicationUser>(model);

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                { return result; }

            IdentityResult roleAssignResult = await _userManager.AddToRoleAsync(user, "Renter");
            if (!roleAssignResult.Succeeded)
                return roleAssignResult;

            var token = await GenerateEmailConfirmationTokenAsync(user);
            var baseUrl = _configuration["AppSettings:BaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured.");
            var confirmationLink = $"{baseUrl}/api/Account/confirm-email?userId={user.Id}&token={token}";
            await _emailService.SendRegistrationConfirmationEmailAsync(user.Email, user.FirstName, confirmationLink);
            return result;
        }

        private async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            return encodedToken;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(Guid userId, string token)
        {
           if (userId == Guid.Empty || string.IsNullOrEmpty(token))
               return IdentityResult.Failed(new IdentityError { Description = "Invalid token or user ID." });

           var user = await _userManager.FindByIdAsync(userId.ToString());

           if (user == null)
               return IdentityResult.Failed(new IdentityError { Description = "User not found." });

           var decodedBytes = WebEncoders.Base64UrlDecode(token);
           var decodedToken = Encoding.UTF8.GetString(decodedBytes);
           var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

           if (result.Succeeded)
           {
               var baseUrl = _configuration["AppSettings:BaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured.");
               var loginLink = $"{baseUrl}/Account/Login";
               await _emailService.SendAccountCreatedEmailAsync(user.Email!, user.FirstName!, loginLink);
           }

           return result;
        }

        public async Task ResendEmailConfirmationAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                // Prevent user enumeration by not disclosing existence
                return;
            }
            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                // Email already confirmed; no action needed
                return;
            }

            var token = await GenerateEmailConfirmationTokenAsync(user);
            var baseUrl = _configuration["AppSettings:BaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured.");
            var confirmationLink = $"{baseUrl}/Account/ConfirmEmail?userId={user.Id}&token={token}";
            await _emailService.SendResendConfirmationEmailAsync(user.Email!, user.FirstName!, confirmationLink);
        }

        //public async Task<bool> CheckEmailTakenAlready(string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null) return true;
        //    return false;
        //}


public async Task<ServiceResult<bool>> ForgotPasswordAsync(ForgotPasswordRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user != null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(token)
            );

            var resetLink = $"https://yourfrontend.com/reset-password?email={user.Email}&token={encodedToken}";

            await _emailService.SendResetPasswordEmailAsync(user.Email, resetLink);
        }

        // Always return OK (security)
        return ServiceResult<bool>.Ok(true, "If the email exists, a reset link was sent.");
    }

    public async Task<ServiceResult<bool>> ValidateResetTokenAsync(ValidateResetTokenRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return ServiceResult<bool>.Fail("Invalid request.");

        var decodedToken = Encoding.UTF8.GetString(
            WebEncoders.Base64UrlDecode(request.Token)
        );

        var isValid = await _userManager.VerifyUserTokenAsync(
            user,
            TokenOptions.DefaultProvider,
            "ResetPassword",
            decodedToken
        );

        return isValid
            ? ServiceResult<bool>.Ok(true)
            : ServiceResult<bool>.Fail("Invalid or expired token.");
    }

    public async Task<ServiceResult<bool>> ResetPasswordAsync(ResetPasswordRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return ServiceResult<bool>.Fail("Invalid request.");

        var decodedToken = Encoding.UTF8.GetString(
            WebEncoders.Base64UrlDecode(request.Token)
        );

        var result = await _userManager.ResetPasswordAsync(
            user,
            decodedToken,
            request.NewPassword
        );

        if (!result.Succeeded)
        {
            return ServiceResult<bool>.Fail(
                "Password reset failed.",
                result.Errors.Select(e => e.Description).ToList()
            );
        }

        return ServiceResult<bool>.Ok(true, "Password reset successful.");
    }





        ////
    }
}
