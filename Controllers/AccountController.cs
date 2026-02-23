using MARN_API.DTOs;
using MARN_API.Services.Implementations;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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


        /// <summary>
        /// login the user and return the token
        /// </summary>
        /// <param name="logInDto">User login information</param>
        /// <returns>Success message</returns>
        /// <response code="200">Returns success message if login is successful</response>
        /// <response code="400">If the request is invalid or login fails</response>
        /// <response code="401">If the user is not found</response>
        /// <response code="429">If rate limit is exceeded</response>
        [HttpPost("login")]
        [EnableRateLimiting("StrictAuth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<TokenDto>> Login(LogInDto logInDto)
        {
            _logger.LogInformation("Login attempt for email: {Email}", logInDto.Email);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Login failed: Invalid model state for email: {Email}", logInDto.Email);
                return BadRequest(ModelState);
            }

            var user = await _accountService.GetUserByEmailAsync(logInDto.Email);

            if (user == null)
            {
                _logger.LogWarning("Login failed: User not found for email: {Email}", logInDto.Email);
                return Unauthorized("Invalid email or password");
            }

            if (user.EmailConfirmed == false)
            {
                _logger.LogWarning("Login failed: Email not confirmed for user: {UserId}", user.Id);
                return Unauthorized("Email not confirmed. Please check your email for confirmation instructions.");
            }

            bool isValid = await _accountService.CheckPasswordAsync(user, logInDto.Password);

            if (!isValid)
            {
                _logger.LogWarning("Login failed: Invalid password for user: {UserId}", user.Id);
                return Unauthorized("Invalid email or password");
            }

            var roles = await _accountService.GetUserRolesAsync(user);
            var expiration = DateTime.UtcNow.AddDays(7);
            var tokenString = _tokenService.CreateToken(user, roles, expiration);

            _logger.LogInformation("Login successful for user: {UserId}", user.Id);

            return Ok(new TokenDto
            {
                Token = tokenString,
                Expiration = expiration
            });
        }

        /// <summary>
        /// Registers a new user account
        /// </summary>
        /// <param name="RegisterDto">User registration information</param>
        /// <returns>Success message</returns>
        /// <response code="200">Returns success message if registration is successful</response>
        /// <response code="400">If the request is invalid or registration fails</response>
        /// <response code="429">If rate limit is exceeded</response>
        [HttpPost("register")]
        [EnableRateLimiting("StrictAuth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Register([FromBody] RegisterDto RegisterDto)
        {
            _logger.LogInformation("Registration attempt for email: {Email}", RegisterDto.Email);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Registration failed: Invalid model state for email: {Email}", RegisterDto.Email);
                return BadRequest(ModelState);
            }

            var result = await _accountService.RegisterUserAsync(RegisterDto);

            if (result.Succeeded)
            {
                _logger.LogInformation("Registration successful for email: {Email}", RegisterDto.Email);
                return Ok(new
                {
                    Message = "Registration successful. Please confirm your email.",
                    Email = RegisterDto.Email
                });
            }

            // Add Identity errors to ModelState
            foreach (var error in result.Errors)
            {
                _logger.LogWarning("Registration failed: {ErrorCode} - {ErrorDescription} for email: {Email}",
                    error.Code, error.Description, RegisterDto.Email);
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Resend Confirmation Email to the user
        /// </summary>
        /// <param name="email">User registration email</param>
        /// <returns>Success message</returns>
        /// <response code="200">Returns success message if Confirmaiton Email Resending is successful</response>
        /// <response code="400">If the user email is already confirmed</response>
        /// <response code="401">If the user email is not found</response>
        /// <response code="429">If rate limit is exceeded</response>
        [HttpPost("resend-confirmation-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] string email)
        {
            _logger.LogInformation("Resend Confirmation Email attempt for email: {Email}", email);

            var user = await _accountService.GetUserByEmailAsync(email);

            if (user == null)
            {
                _logger.LogWarning("Resend Confirmation Email failed: User not found for email: {Email}", email);
                return Unauthorized("Unknown user: Please Regester your account first.");
            }

            if (user.EmailConfirmed == true)
            {
                _logger.LogWarning("Resend Confirmation Email failed: Email is confirmed for user: {UserId}", user.Id);
                return BadRequest("Email is already confirmed. Please try to login with your account.");
            }

            await _accountService.ResendEmailConfirmationAsync(email);
            return Ok(new
            {
                Message = "A confirmation email has been sent. Please check your inbox."
            });
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

            // 1. Validation
            if (userId == Guid.Empty || string.IsNullOrWhiteSpace(token))
            {
                _logger.LogWarning("Email confirmation failed: Invalid userId or token");
                return BadRequest(new { Message = "User ID and Token are required." });
            }

            // 2. Call Service
            var result = await _accountService.ConfirmEmailAsync(userId, token);

            // 3. Handle Success
            if (result.Succeeded)
            {
                _logger.LogInformation("Email confirmed successfully for user: {UserId}", userId);
                return Ok(new { Message = "Email confirmed successfully!" });
            }

            // 4. Handle Identity Failures (Expired token, invalid token, etc.)
            _logger.LogWarning("Email confirmation failed for user: {UserId}. Errors: {Errors}",
                userId, string.Join(", ", result.Errors.Select(e => e.Description)));

            return BadRequest(new
            {
                Message = "Email confirmation failed.",
                Errors = result.Errors.Select(e => e.Description)
            });
        }

        /// <summary>
        /// Initiates the password reset process by sending a reset link to the user's email address.
        /// </summary>
        /// <param name="request">Contains the user's email address.</param>
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
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto request)
        {
            var result = await _accountService.ForgotPasswordAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Resets the user's password using a valid password reset token.
        /// </summary>
        /// <param name="request">
        /// Contains the user's email address, reset token, new password, and confirmation password.
        /// </param>
        /// <returns>
        /// Success message if the password is successfully reset.
        /// </returns>
        /// <response code="200">
        /// Returns success message if the password was reset successfully.
        /// </response>
        /// <response code="400">
        /// If the reset token is invalid, expired, or the password reset fails.
        /// </response>
        /// <response code="429">
        /// If the rate limit is exceeded.
        /// </response>
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> ResetPassword(
            [FromBody] ResetPasswordRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.ResetPasswordAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Validates the password reset token before allowing the user to set a new password.
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
        /// If the token is invalid, expired, or the request is malformed.
        /// </response>
        /// <response code="429">
        /// If the rate limit is exceeded.
        /// </response>
        [HttpPost("validate-reset-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> ValidateResetToken(
        [FromBody] ValidateResetTokenRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.ValidateResetTokenAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
