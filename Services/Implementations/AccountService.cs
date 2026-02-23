using AutoMapper;
using MARN_API.DTOs;
using MARN_API.Enums;
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
        private readonly ITokenService _tokenService;
        private readonly ILogger<AccountService> _logger;
        private readonly IMapper _mapper;
        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            IConfiguration configuration,
            IMapper mapper
,
            ITokenService tokenService,
            ILogger<AccountService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _configuration = configuration;
            _mapper = mapper;
            _tokenService = tokenService;
            _logger = logger;
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

            if (!await _userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword))
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

        ////////

        public async Task<ServiceResult<LoginResponseDto>> LoginAsync(LogInDto dto)
        {
            _logger.LogInformation("Login attempt for email: {Email}", dto.Email);

            var user = await GetUserByEmailAsync(dto.Email);

            if (user == null)
            {
                _logger.LogWarning("Login failed: User not found for email: {Email}", dto.Email);
                return ServiceResult<LoginResponseDto>.Fail("Invalid email or password");
            }

            if (!user.EmailConfirmed)
            {
                _logger.LogWarning("Login failed: Email not confirmed for user: {UserId}", user.Id);
                return ServiceResult<LoginResponseDto>.Fail(
                    "Email not confirmed. Please check your email for confirmation instructions.");
            }
            if (await _userManager.IsLockedOutAsync(user))
            {
                _logger.LogWarning("Login failed: User locked out: {UserId}", user.Id);

                return ServiceResult<LoginResponseDto>.Fail(
                    "Account is locked. Try again later."
                );
            }
            bool isValid = await CheckPasswordAsync(user, dto.Password);

            if (!isValid)
            {
                await _userManager.AccessFailedAsync(user);
                _logger.LogWarning("Login failed: Invalid password for user: {UserId}", user.Id);
                return ServiceResult<LoginResponseDto>.Fail("Invalid email or password");
            }
            await _userManager.ResetAccessFailedCountAsync(user);

            if (await GetTwoFactorEnabledAsync(user))
            {
                var code = await GenerateTwoFactorTokenAsync(user);

                await _emailService.Send2FAEmailAsync(user.Email, "2FA Code - MARN", code);
                _logger.LogInformation("Sent 2FA code to user: {UserId}", user.Id);

                return ServiceResult<LoginResponseDto>.Ok(new LoginResponseDto
                {
                    RequiresTwoFactor = true,
                    TwoFactorProvider = "Email"
                }, resultType: ServiceResultType.RequiresTwoFactor);
            }

            var roles = await GetUserRolesAsync(user);

            var expiration = DateTime.UtcNow.AddDays(7);
            var tokenString = _tokenService.CreateToken(user, roles, expiration);

            _logger.LogInformation("Login successful for user: {UserId}", user.Id);

            return ServiceResult<LoginResponseDto>.Ok(new LoginResponseDto
            {
                Token = tokenString,
                Expiration = expiration,
                RequiresTwoFactor = false
            });
        }

        public async Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return await _userManager.GetTwoFactorEnabledAsync(user);
        }

        public async Task<string> GenerateTwoFactorTokenAsync(ApplicationUser user)
        {
            return await _userManager.GenerateTwoFactorTokenAsync(
                user,
                TokenOptions.DefaultEmailProvider
            );
        }


        ////
        public async Task<ServiceResult<LoginResponseDto>> VerifyTwoFactorAsync(VerifyTwoFactorDto dto)
        {
            var user = await GetUserByEmailAsync(dto.Email);

            if (user == null)
                return ServiceResult<LoginResponseDto>.Fail("Invalid request");

            // Optional: lockout check
            if (await _userManager.IsLockedOutAsync(user))
            {
                return ServiceResult<LoginResponseDto>.Fail("Account is locked. Try again later.");
            }

            var isValid = await _userManager.VerifyTwoFactorTokenAsync(
                user,
                TokenOptions.DefaultEmailProvider,
                dto.Code
            );

            if (!isValid)
            {
                // increment failed attempts
                await _userManager.AccessFailedAsync(user);
                return ServiceResult<LoginResponseDto>.Fail("Invalid verification code");
            }

            // reset failed attempts after success
            await _userManager.ResetAccessFailedCountAsync(user);

            // Generate JWT with MFA claim
            var roles = await GetUserRolesAsync(user);
            var expiration = DateTime.UtcNow.AddDays(7);

            var token = _tokenService.CreateToken(user, roles, expiration, includeMfaClaim: true);

            return ServiceResult<LoginResponseDto>.Ok(new LoginResponseDto
            {
                Token = token,
                Expiration = expiration,
                RequiresTwoFactor = false
            }, resultType: ServiceResultType.Success);
        }


        public async Task<ServiceResult<bool>> ToggleTwoFactorAsync(string userId, string? password = null)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ServiceResult<bool>.Fail("User not found");

            // Optional: verify password before allowing toggle
            if (!string.IsNullOrEmpty(password))
            {
                if (!await _userManager.CheckPasswordAsync(user, password))
                    return ServiceResult<bool>.Fail("Invalid password");
            }

            bool newState = !user.TwoFactorEnabled;

            var result = await _userManager.SetTwoFactorEnabledAsync(user, newState);
            if (!result.Succeeded)
                return ServiceResult<bool>.Fail("Failed to toggle 2FA", result.Errors.Select(e => e.Description).ToList());

            _logger.LogInformation("User {UserId} toggled 2FA. Enabled={Enabled}", user.Id, newState);

            return ServiceResult<bool>.Ok(newState, $"Two-Factor Authentication is now {(newState ? "enabled" : "disabled")}");
        }
        ////
    }
}
