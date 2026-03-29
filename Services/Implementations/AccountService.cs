using AutoMapper;
using Google.Apis.Auth;
using MARN_API.DTOs.Auth;
using MARN_API.Enums;
using MARN_API.Enums.Account;
using MARN_API.Models;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Web.Http.ModelBinding;

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
            IMapper mapper,
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


        #region Login And 2FA
        public async Task<ServiceResult<LoginResponseDto>> LoginAsync(LogInDto dto)
        {
            _logger.LogInformation("Login attempt for email: {Email}", dto.Email);

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                _logger.LogWarning("Login failed: User not found for email: {Email}", dto.Email);
                return ServiceResult<LoginResponseDto>.Fail(
                    "Invalid email or password", 
                    resultType: ServiceResultType.Unauthorized
                );
            }

            if (!user.EmailConfirmed)
            {
                _logger.LogWarning("Login failed: Email not confirmed for user: {UserId}", user.Id);
                return ServiceResult<LoginResponseDto>.Fail(
                    "Email not confirmed. Please check your email for confirmation instructions.",
                    resultType: ServiceResultType.Unauthorized
                );
            }

            bool isLockedOut = await _userManager.IsLockedOutAsync(user);
            if (isLockedOut)
            {
                _logger.LogWarning("Login failed: User locked out: {UserId}", user.Id);
                return ServiceResult<LoginResponseDto>.Fail(
                    "Account is locked. Try again later.",
                    resultType: ServiceResultType.Unauthorized
                );
            }

            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordValid)
            {
                await _userManager.AccessFailedAsync(user);

                _logger.LogWarning("Login failed: Invalid password for user: {UserId}", user.Id);
                return ServiceResult<LoginResponseDto>.Fail(
                    "Invalid email or password", 
                    resultType: ServiceResultType.Unauthorized
                );
            }

            bool isDeleted = user.DeletedAt != null;
            if (isDeleted)
            {
                if (user.AccountStatus == AccountStatus.Banned)
                {
                    _logger.LogWarning("Login failed: Account banned for user: {UserId}", user.Id);
                    return ServiceResult<LoginResponseDto>.Fail(
                        "Account has been banned. Contact support for assistance.",
                        resultType: ServiceResultType.Unauthorized
                    );
                }
                _logger.LogWarning("Login failed: Account deleted for user: {UserId}", user.Id);
                return ServiceResult<LoginResponseDto>.Fail(
                    "Account has been deleted. Contact support for assistance.",
                    resultType: ServiceResultType.Unauthorized
                );
            }

            await _userManager.ResetAccessFailedCountAsync(user);

            bool isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            if (isTwoFactorEnabled)
            {
                var code = await _userManager.GenerateTwoFactorTokenAsync(
                    user,
                    TokenOptions.DefaultEmailProvider
                );

                await _emailService.Send2FAEmailAsync(user.Email!, "2FA Code - MARN", code);

                _logger.LogInformation("Sent 2FA code to user: {UserId}", user.Id);
                return ServiceResult<LoginResponseDto>.Ok(new LoginResponseDto
                {
                    RequiresTwoFactor = true,
                    TwoFactorProvider = "Email"
                },
                "Two-Factor Authentication is required. A verification code has been sent to your email.",
                resultType: ServiceResultType.RequiresTwoFactor);
            }

            var response = await CreateJwtForUserAsync(user, dto.RememberMe);
            return ServiceResult<LoginResponseDto>.Ok(response);
        }

        private async Task<LoginResponseDto> CreateJwtForUserAsync(ApplicationUser user, bool rememberMe, string provider = null!)
        {
            var roles = await _userManager.GetRolesAsync(user);

            DateTime expiration;
            if (rememberMe)
                expiration = DateTime.UtcNow.AddDays(30);
            else
                expiration = DateTime.UtcNow.AddDays(7);

            var tokenString = _tokenService.CreateToken(user, roles, expiration);

            bool isExternal = provider != null;

            _logger.LogInformation("Login successful for user: {UserId} by provider: {provider}", user.Id, provider);
            return new LoginResponseDto
            {
                Token = tokenString,
                Expiration = expiration,
                RequiresTwoFactor = false,
                IsExternalLogin = isExternal,
                ExternalProvider = provider
            };
        }

        public async Task<ServiceResult<LoginResponseDto>> GoogleLoginAsync(GoogleLoginDto dto)
        {
            _logger.LogInformation("Google login attempt started.");

            var payload = await ValidateGoogleTokenAsync(dto.IdToken);

            if (payload == null)
            {
                _logger.LogWarning("Google login failed: Invalid token");
                return ServiceResult<LoginResponseDto>
                    .Fail("Invalid Google token", resultType: ServiceResultType.Unauthorized);
            }

            if (!payload.EmailVerified)
            {
                _logger.LogWarning("Google login failed: Email not verified by provider");
                return ServiceResult<LoginResponseDto>
                    .Fail("Google email not verified", resultType: ServiceResultType.Unauthorized);
            }

            var user = await _userManager.FindByLoginAsync("Google", payload.Subject);

            if (user == null)
            {
                _logger.LogInformation("No existing Google login found. Checking email: {Email}", payload.Email);

                user = await _userManager.FindByEmailAsync(payload.Email);

                if (user == null)
                {
                    _logger.LogInformation("Creating new user for Google email: {Email}", payload.Email);

                    user = new ApplicationUser
                    {
                        Email = payload.Email,
                        UserName = payload.Email,
                        EmailConfirmed = true
                    };

                    var createResult = await _userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                    {
                        _logger.LogWarning(
                            "Failed to create user for Google login with email: {Email}. Errors: {@Errors}",
                            payload.Email,
                            createResult.Errors.Select(e => e.Description)
                        );
                        return ServiceResult<LoginResponseDto>.Fail(
                            "User creation failed", 
                            createResult.Errors.Select(e => e.Description).ToList(),
                            ServiceResultType.Conflict
                        );
                    }
                }

                var logins = await _userManager.GetLoginsAsync(user);
                if (!logins.Any(l => l.LoginProvider == "Google"))
                {
                    var addLoginResult = await _userManager.AddLoginAsync(
                        user,
                        new UserLoginInfo("Google", payload.Subject, "Google"));

                    if (!addLoginResult.Succeeded)
                    {
                        _logger.LogError(
                            "Failed to link Google login to user {UserId}. Errors: {@Errors}",
                            user.Id,
                            addLoginResult.Errors.Select(e => e.Description));

                        return ServiceResult<LoginResponseDto>.Fail(
                            "External login failed.",
                            addLoginResult.Errors.Select(e => e.Description).ToList(),
                            ServiceResultType.Conflict
                        );
                    }
                }

                _logger.LogInformation("Google account linked successfully to user {UserId}", user.Id);
            }

            bool isLocked = await _userManager.IsLockedOutAsync(user);
            if (isLocked)
            {
                _logger.LogWarning("Google login failed: User locked out: {UserId}", user.Id);
                return ServiceResult<LoginResponseDto>
                    .Fail("Account is locked. Try again later.", resultType: ServiceResultType.Unauthorized);
            }

            bool isDeleted = user.DeletedAt != null;
            if (isDeleted)
            {
                if (user.AccountStatus == AccountStatus.Banned)
                {
                    _logger.LogWarning("Login failed: Account banned for user: {UserId}", user.Id);
                    return ServiceResult<LoginResponseDto>.Fail(
                        "Account has been banned. Contact support for assistance.",
                        resultType: ServiceResultType.Unauthorized
                    );
                }
                _logger.LogWarning("Login failed: Account deleted for user: {UserId}", user.Id);
                return ServiceResult<LoginResponseDto>.Fail(
                    "Account has been deleted. Contact support for assistance.",
                    resultType: ServiceResultType.Unauthorized
                );
            }

            var response = await CreateJwtForUserAsync(user, dto.RememberMe, "Google");
            return ServiceResult<LoginResponseDto>.Ok(response);
        }

        private async Task<GoogleJsonWebSignature.Payload?> ValidateGoogleTokenAsync(string idToken)
        {
            _logger.LogInformation("Validate Google Token attempt.");

            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _configuration["Authentication:Google:ClientId"] }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                _logger.LogInformation("Google token validation successed.");
                return payload;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Google token validation failed.");
                return null;
            }
        }

        public async Task<ServiceResult<LoginResponseDto>> VerifyTwoFactorAsync(VerifyTwoFactorDto dto)
        {
            _logger.LogInformation("Verify2FA attempt for email: {Email}", dto.Email);

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                _logger.LogWarning("Verify2FA failed: User not found for email: {Email}", dto.Email);
                return ServiceResult<LoginResponseDto>.Fail("Invalid request", resultType: ServiceResultType.Unauthorized);
            }

            // Optional: lockout check
            if (await _userManager.IsLockedOutAsync(user))
            {
                _logger.LogWarning("Verify2FA failed: User locked out: {UserId}", user.Id);
                return ServiceResult<LoginResponseDto>.Fail("Account is locked. Try again later.", resultType: ServiceResultType.Unauthorized);
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

                _logger.LogWarning("Verify2FA failed: Invalid verification code for user: {UserId}", user.Id);
                return ServiceResult<LoginResponseDto>.Fail("Invalid verification code", resultType: ServiceResultType.BadRequest);
            }

            var response = await CreateJwtForUserAsync(user, dto.RememberMe);
            return ServiceResult<LoginResponseDto>.Ok(response);
        }
        #endregion


        #region Register And Confirm Email
        public async Task<ServiceResult<bool>> RegisterUserAsync(RegisterDto dto)
        {
            _logger.LogInformation("Register attempt for email: {Email}", dto.Email);

            var user = _mapper.Map<ApplicationUser>(dto);

            IdentityResult result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                _logger.LogWarning(
                    "Registration failed for {Email}. Errors: {@Errors}",
                    user.Email,
                    result.Errors.Select(e => e.Description)
                );
                return ServiceResult<bool>.Fail("Failed to Register", result.Errors.Select(e => e.Description).ToList());
            }

            IdentityResult roleAssignResult = await _userManager.AddToRoleAsync(user, "Renter");
            if (!roleAssignResult.Succeeded)
            {
                _logger.LogError(
                    "Registration failed for {Email}. Errors: {@Errors}",
                    user.Email,
                    roleAssignResult.Errors.Select(e => e.Description)
                );
                return ServiceResult<bool>.Fail("Failed to Register", roleAssignResult.Errors.Select(e => e.Description).ToList());
            }

            var token = await GenerateEmailConfirmationTokenAsync(user);

            var frontBaseUrl = _configuration["AppSettings:FrontBaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured.");
            var confirmationLink = $"{frontBaseUrl}/Account/confirm-email?userId={user.Id}&token={token}";

            await _emailService.SendRegistrationConfirmationEmailAsync(user.Email!, user.FirstName, confirmationLink);

            _logger.LogInformation("Registration successful for email: {Email}", user.Email);
            return ServiceResult<bool>
                .Ok(true, "Registration successful. Please confirm your email.", resultType: ServiceResultType.Created);
        }

        private async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            return encodedToken;
        }

        public async Task<ServiceResult<bool>> ConfirmEmailAsync(Guid userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                _logger.LogWarning("Confirm Email failed: User not found for userId: {userId}", userId);
                return ServiceResult<bool>.Fail("Failed to Confirm Email: User not found.");
            }

            var decodedBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedBytes);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
            {
                var frontBaseUrl = _configuration["AppSettings:FrontBaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured.");
                var loginLink = $"{frontBaseUrl}/Account/Login";

                await _emailService.SendAccountCreatedEmailAsync(user.Email!, user.FirstName!, loginLink);

                _logger.LogInformation("Email confirmed successfully for user: {UserId}", userId);
                return ServiceResult<bool>.Ok(true, "The Email Confirmed Successfully");
            }
            else
            {
                _logger.LogWarning(
                    "Confirm Email for {Email} failed. Errors: {@Errors}",
                    user.Email,
                    result.Errors.Select(e => e.Description)
                );
                return ServiceResult<bool>.Fail("Failed to Confirm Email", result.Errors.Select(e => e.Description).ToList());
            }
        }

        public async Task<ServiceResult<bool>> ResendEmailConfirmationAsync(ResendConfirmationEmailDto dto)
        {
            _logger.LogInformation("Resend Confirmation Email attempt for email: {Email}", dto.Email);

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user != null)
            {
                var isConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                if (!isConfirmed) 
                {
                    var token = await GenerateEmailConfirmationTokenAsync(user);
                    var frontBaseUrl = _configuration["AppSettings:FrontBaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured.");
                    var confirmationLink = $"{frontBaseUrl}/Account/confirm-email?userId={user.Id}&token={token}";

                    await _emailService.SendResendConfirmationEmailAsync(user.Email!, user.FirstName!, confirmationLink);
                    _logger.LogInformation("Confirmation email resent for user {UserId}", user.Id);
                }
            }

            return ServiceResult<bool>.Ok(true, "If the email exists and not confirmed, a confirmation email has been sent. Please check your inbox.");
        }
        #endregion


        #region Reset Password
        public async Task<ServiceResult<bool>> ForgotPasswordAsync(ForgotPasswordRequestDto dto)
        {
            _logger.LogInformation("Forgot Password attempt for email: {Email}", dto.Email);

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                _logger.LogWarning("Forgot Password failed: User not found for email: {Email}", dto.Email);
            else
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var encodedToken = WebEncoders.Base64UrlEncode(
                    Encoding.UTF8.GetBytes(token)
                );

                var frontBaseUrl = _configuration["AppSettings:FrontBaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured.");
                var resetLink = $"{frontBaseUrl}/reset-password?email={user.Email}&token={encodedToken}";

                await _emailService.SendResetPasswordEmailAsync(user.Email!, user.FirstName, resetLink);

                _logger.LogInformation("Forgot Password Email successfully sent for user: {UserId}", user.Id);
            }

            // Always return OK (security)
            return ServiceResult<bool>.Ok(true, "If the email exists, a reset link was sent.");
        }

        public async Task<ServiceResult<bool>> ValidateResetTokenAsync(ValidateResetTokenRequestDto dto)
        {
            _logger.LogInformation("Validate Reset Token attempt for email: {Email}", dto.Email);

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                _logger.LogWarning("Validate Reset Token failed: User not found for email: {Email}", dto.Email);
                return ServiceResult<bool>.Fail("Invalid request.", resultType: ServiceResultType.Unauthorized);
            }

            var decodedToken = Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(dto.Token)
            );

            var isValidToken = await _userManager.VerifyUserTokenAsync(
                user,
                _userManager.Options.Tokens.PasswordResetTokenProvider,
                "ResetPassword",
                decodedToken
            );

            if (!isValidToken)
            {
                _logger.LogWarning(
                    "Invalid or expired reset token for email: {Email}",
                    dto.Email
                );
                return ServiceResult<bool>.Fail(
                    "Invalid or expired token.",
                    resultType: ServiceResultType.Unauthorized
                );
            }

            _logger.LogInformation("Validate Reset Token successful for user: {UserId}", user.Id);
            return ServiceResult<bool>.Ok(true, "Valid token");
        }

        public async Task<ServiceResult<bool>> ResetPasswordAsync(ResetPasswordRequestDto dto)
        {
            _logger.LogInformation("Reset Password attempt for email: {Email}", dto.Email);

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                _logger.LogWarning("Reset Password failed: User not found for email: {Email}", dto.Email);
                return ServiceResult<bool>.Fail("Invalid request.", resultType: ServiceResultType.Unauthorized);
            }

            var decodedToken = Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(dto.Token)
            );

            var isValidToken = await _userManager.VerifyUserTokenAsync(
                user,
                _userManager.Options.Tokens.PasswordResetTokenProvider,
                "ResetPassword",
                decodedToken
            );

            if (!isValidToken)
            {
                _logger.LogWarning(
                    "Invalid or expired reset token for email: {Email}",
                    dto.Email
                );
                return ServiceResult<bool>.Fail(
                    "Invalid or expired token.",
                    resultType: ServiceResultType.Unauthorized
                );
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                decodedToken,
                dto.NewPassword
            );

            if (!result.Succeeded)
            {
                _logger.LogError(
                    "Reset Password failed for {Email}. Errors: {@Errors}",
                    user.Email,
                    result.Errors.Select(e => e.Description)
                );
                return ServiceResult<bool>.Fail(
                    "Password reset failed.",
                    result.Errors.Select(e => e.Description).ToList(),
                    resultType: ServiceResultType.BadRequest
                );
            }

            _logger.LogInformation("Password reset successful for user: {UserId}", user.Id);
            return ServiceResult<bool>.Ok(true, "Password reset successful.");
        }
        #endregion
    }
}
