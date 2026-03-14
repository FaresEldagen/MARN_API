using MARN_API.DTOs.Auth;
using MARN_API.Enums;
using MARN_API.Models;
using MARN_API.Services.Implementations;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MARN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IAccountService accountService,
            ITokenService tokenService,
            ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _tokenService = tokenService;
            _logger = logger;
        }


        protected ActionResult HandleServiceResult<T>(ServiceResult<T> result)
        {
            return result.ResultType switch
            {
                ServiceResultType.Success => Ok(new { message = result.Message, data = result.Data }),
                ServiceResultType.Created => StatusCode(201, new { message = result.Message, data = result.Data }),
                ServiceResultType.RequiresTwoFactor => Accepted(new { message = result.Message, data = result.Data }),
                ServiceResultType.Unauthorized => Unauthorized(new { message = result.Message }),
                ServiceResultType.NotFound => NotFound(new { message = result.Message }),
                ServiceResultType.Forbidden => StatusCode(403, new { message = result.Message }),
                ServiceResultType.Conflict => Conflict(new { message = result.Message, errors = result.Errors }),
                _ => BadRequest(new { message = result.Message, errors = result.Errors })
            };
        }


        #region Login And 2FA
        /// <summary>
        /// login the user and return the token
        /// </summary>
        /// <param name="dto">User login information</param>
        /// <returns>JWT token if login is successful</returns>
        /// <response code="200">Returns success message if login is successful</response>
        /// <response code="202">If require 2FA</response>
        /// <response code="400">If the request is invalid or login fails</response>
        /// <response code="401">If the user is not found</response>
        /// <response code="429">If rate limit is exceeded</response>
        [HttpPost("login")]
        [EnableRateLimiting("StrictAuth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<LoginResponseDto>> Login(LogInDto dto)
        {
            var result = await _accountService.LoginAsync(dto);
            return HandleServiceResult<LoginResponseDto>(result);
        }

        /// <summary>
        /// Verify the Two-Factor Authentication (2FA) code sent to the user's email.
        /// </summary>
        /// <param name="dto">User email and 2FA code</param>
        /// <returns>JWT token if verification is successful</returns>
        /// <response code="200">Returns JWT token if 2FA verification is successful</response>
        /// <response code="400">If the request is invalid (bad model)</response>
        /// <response code="401">If verification code is invalid or user not found</response>
        /// <response code="429">If rate limit is exceeded</response>
        [HttpPost("verify-2fa")]
        [EnableRateLimiting("StrictAuth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> VerifyTwoFactor(VerifyTwoFactorDto dto)
        {
            var result = await _accountService.VerifyTwoFactorAsync(dto);
            return HandleServiceResult<LoginResponseDto>(result);
        }

        /// <summary>
        /// Login with Google provider, if the user is logging in with google for the first time, a new account will be created for him and the email will be confirmed by default
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>JWT token if login is successful</returns>
        /// <response code="200">Returns success message if login is successful</response>
        /// <response code="202">If require 2FA</response>
        /// <response code="400">If the request is invalid or login fails</response>
        /// <response code="429">If rate limit is exceeded</response>
        [HttpPost("external/google")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginDto dto)
        {
            var result = await _accountService.GoogleLoginAsync(dto);
            return HandleServiceResult<LoginResponseDto>(result);
        }
        #endregion


        #region Register And Confirm Email
        /// <summary>
        /// Registers a new user account
        /// </summary>
        /// <param name="dto">User registration information</param>
        /// <returns>Success message</returns>
        /// <response code="201">Returns success message if registration is successful</response>
        /// <response code="400">If the request is invalid or registration fails</response>
        /// <response code="429">If rate limit is exceeded</response>
        [HttpPost("register")]
        [EnableRateLimiting("StrictAuth")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _accountService.RegisterUserAsync(dto);
            return HandleServiceResult<bool>(result);
        }

        /// <summary>
        /// Confirms a user's email address using the confirmation token
        /// </summary>
        /// <param name="userId">The unique identifier of the user</param>
        /// <param name="token">The email confirmation token</param>
        /// <returns>Success or error message</returns>
        /// <response code="200">Returns success message if email is confirmed</response>
        /// <response code="400">If the userId or token is invalid or confirmation fails</response>
        /// <response code="429">If rate limit is exceeded</response>
        [HttpGet("confirm-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] Guid userId, [FromQuery] string token)
        {
            _logger.LogInformation("Email confirmation attempt for user: {UserId}", userId);

            if (userId == Guid.Empty || string.IsNullOrWhiteSpace(token))
            {
                _logger.LogWarning("Email confirmation failed: Invalid userId or token");
                return BadRequest(new { Message = "User ID and Token are required." });
            }

            var result = await _accountService.ConfirmEmailAsync(userId, token);
            return HandleServiceResult<bool>(result);
        }

        /// <summary>
        /// Resend Confirmation Email to the user
        /// </summary>
        /// <param name="dto">User registration email</param>
        /// <returns>Success message</returns>
        /// <response code="200">Returns success message if the user email was valid wither Confirmaiton Email Resended successful or not</response>
        /// <response code="400">If the user email is in Invalid format</response>
        /// <response code="429">If rate limit is exceeded</response>
        [HttpPost("resend-confirmation-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailDto dto)
        {
            var result = await _accountService.ResendEmailConfirmationAsync(dto);
            return HandleServiceResult<bool>(result);
        }
        #endregion


        #region Reset Password
        /// <summary>
        /// Initiates the password reset process by sending a reset link to the user's email address.
        /// </summary>
        /// <param name="dto">Contains the user's email address.</param>
        /// <returns>Success message regardless of whether the email exists (for security reasons).</returns>
        /// <response code="200">
        /// Returns a success message indicating that if the email exists, a reset link has been sent.
        /// </response>
        /// <response code="400">
        /// If the request model is invalid (e.g., malformed email).
        /// </response>
        /// <response code="429">
        /// If the rate limit for password reset requests is exceeded.
        /// </response>
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto dto)
        {
            var result = await _accountService.ForgotPasswordAsync(dto);
            return HandleServiceResult<bool>(result);
        }

        /// <summary>
        /// (OPTIONAL) Validates the password reset token before allowing the user to set a new password.
        /// </summary>
        /// <param name="request">
        /// Contains the user's email address and the reset token received via email.
        /// </param>
        /// <returns>
        /// Returns validation result indicating whether the token is valid or expired.
        /// </returns>
        /// <response code="200">
        /// If the reset token is valid.
        /// </response>
        /// <response code="400">
        /// If the request model is invalid (e.g., malformed email).
        /// </response>
        /// <response code="401">
        /// If the user email is not found or the token is invalid or expired.
        /// </response>
        /// <response code="429">
        /// If the rate limit is exceeded.
        /// </response>
        [HttpPost("validate-reset-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> ValidateResetToken([FromBody] ValidateResetTokenRequestDto request)
        {
            var result = await _accountService.ValidateResetTokenAsync(request);
            return HandleServiceResult<bool>(result);
        }

        /// <summary>
        /// Resets the user's password using a valid password reset token.
        /// </summary>
        /// <param name="dto">
        /// Contains the user's email address, reset token, new password, and confirmation password.
        /// </param>
        /// <returns>
        /// Success message if the password is successfully reset.
        /// </returns>
        /// <response code="200">
        /// Returns success message if the password was reset successfully.
        /// </response>
        /// <response code="400">
        /// If the request model is invalid (e.g., malformed email), or If the password reset fails to update in the database.
        /// </response>
        /// <response code="401">
        /// If the user email is not found or the token is invalid or expired.
        /// </response>
        /// <response code="429">
        /// If the rate limit is exceeded.
        /// </response>
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
        {
            var result = await _accountService.ResetPasswordAsync(dto);
            return HandleServiceResult<bool>(result);
        }
        #endregion


        #region For Testing
        [Authorize]
        [HttpGet("auth")]
        public IActionResult testRequireAuthEndPoint()
        {
            return Ok("works");
        }

        [HttpGet("notAuth")]
        public IActionResult testNotRequireAuthEndPoint()
        {
            return Ok("works");
        }
        #endregion
    }
}
