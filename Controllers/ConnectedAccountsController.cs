//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using MARN_API.DTOs.Common;
//using MARN_API.DTOs.ConnectedAccounts;
//using MARN_API.Models;
//using MARN_API.Services.Interfaces;
//using System.Security.Cryptography;
//using System.Text;

//namespace MARN_API.Controllers
//{
//    [ApiController]
//    [Route("api/connected-accounts")]
//    public class ConnectedAccountsController : BaseController
//    {
//        private readonly IConnectedAccountService _connectedAccountService;
//        private readonly IConfiguration _configuration;
//        private readonly ILogger<ConnectedAccountsController> _logger;

//        public ConnectedAccountsController(IConnectedAccountService connectedAccountService, IConfiguration configuration, ILogger<ConnectedAccountsController> logger)
//        {
//            _connectedAccountService = connectedAccountService;
//            _configuration = configuration;
//            _logger = logger;
//        }

//        /// <summary>
//        /// Returns all connected Stripe accounts for testing and inspection.
//        /// </summary>
//        /// <response code="200">Returns all connected account mappings.</response>
//        [HttpGet]
//        [ProducesResponseType(typeof(IEnumerable<ConnectedAccountResponseDto>), StatusCodes.Status200OK)]
//        public async Task<IActionResult> GetAllConnectedAccounts()
//        {
//            var connectedAccounts = await _connectedAccountService.GetAllConnectedAccountsAsync();
//            return Ok(connectedAccounts.Select(MapResponse));
//        }

//        /// <summary>
//        /// Returns the connected Stripe account that belongs to the current owner.
//        /// </summary>
//        /// <response code="200">Returns the current owner's connected account.</response>
//        /// <response code="401">If the user is not authenticated.</response>
//        /// <response code="404">If no connected account exists for the current owner.</response>
//        [Authorize(Roles = "Owner")]
//        [HttpGet("me")]
//        [ProducesResponseType(typeof(ConnectedAccountResponseDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> GetMyConnectedAccount()
//        {
//            if (!TryGetUserId(out var userId))
//            {
//                return Unauthorized("User ID not found in token");
//            }

//            var connectedAccount = await _connectedAccountService.GetConnectedAccountByApplicationUserIdAsync(userId);
//            if (connectedAccount is null)
//            {
//                return NotFound(new { message = "Connected account not found" });
//            }

//            return Ok(MapResponse(connectedAccount));
//        }

//        /// <summary>
//        /// Returns whether Stripe onboarding is complete for the current owner.
//        /// </summary>
//        /// <response code="200">Returns the current onboarding completion state.</response>
//        /// <response code="401">If the user is not authenticated.</response>
//        /// <response code="404">If no connected account exists for the current owner.</response>
//        [Authorize(Roles = "Owner")]
//        [HttpGet("me/onboarding-status")]
//        [ProducesResponseType(typeof(ConnectedAccountOnboardingStatusResponseDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> GetMyOnboardingStatus()
//        {
//            if (!TryGetUserId(out var userId))
//            {
//                return Unauthorized("User ID not found in token");
//            }

//            var connectedAccount = await _connectedAccountService.GetConnectedAccountByApplicationUserIdAsync(userId);
//            if (connectedAccount is null)
//            {
//                return NotFound(new { message = "Connected account not found" });
//            }

//            var isComplete = await _connectedAccountService.VerifyOnboardingCompletionAsync(connectedAccount.Id);
//            return Ok(new ConnectedAccountOnboardingStatusResponseDto
//            {
//                ConnectedAccountId = connectedAccount.Id,
//                ApplicationUserId = connectedAccount.ApplicationUserId,
//                StripeAccountId = connectedAccount.StripeAccountId,
//                IsOnboardingComplete = isComplete
//            });
//        }

//        /// <summary>
//        /// Creates a connected Stripe account for the authenticated owner and returns an onboarding link.
//        /// </summary>
//        /// <response code="200">Returns the created connected account and Stripe onboarding URL.</response>
//        /// <response code="401">If the user is not authenticated.</response>
//        /// <response code="500">If account creation fails unexpectedly.</response>
//        [Authorize(Roles = "Owner")]
//        [HttpPost]
//        [ProducesResponseType(typeof(ConnectedAccountOnboardingLinkResponseDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<IActionResult> CreateConnectedAccount()
//        {
//            if (!TryGetUserId(out var userId))
//            {
//                return Unauthorized("User ID not found in token");
//            }

//            try
//            {
//                var connectedAccount = await _connectedAccountService.CreateConnectedAccountAsync(userId);
//                var onboardingUrl = await _connectedAccountService.GenerateOnboardingLinkAsync(
//                    connectedAccount.Id,
//                    BuildOnboardingReturnUrl(connectedAccount),
//                    BuildOnboardingRefreshUrl(connectedAccount));

//                return Ok(new ConnectedAccountOnboardingLinkResponseDto
//                {
//                    ConnectedAccountId = connectedAccount.Id,
//                    ApplicationUserId = connectedAccount.ApplicationUserId,
//                    StripeAccountId = connectedAccount.StripeAccountId,
//                    OnboardingUrl = onboardingUrl
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Failed to create connected account for user {UserId}", userId);
//                return StatusCode(500, new { error = "Failed to create connected account." });
//            }
//        }

//        /// <summary>
//        /// Generates a new Stripe onboarding link for the current owner.
//        /// </summary>
//        /// <response code="200">Returns a fresh Stripe onboarding link.</response>
//        /// <response code="401">If the user is not authenticated.</response>
//        /// <response code="404">If no connected account exists for the current owner.</response>
//        [Authorize(Roles = "Owner")]
//        [HttpPost("me/onboarding-link")]
//        [ProducesResponseType(typeof(ConnectedAccountOnboardingLinkResponseDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> CreateMyOnboardingLink()
//        {
//            if (!TryGetUserId(out var userId))
//            {
//                return Unauthorized("User ID not found in token");
//            }

//            var connectedAccount = await _connectedAccountService.GetConnectedAccountByApplicationUserIdAsync(userId);
//            if (connectedAccount is null)
//            {
//                return NotFound(new { message = "Connected account not found" });
//            }

//            var onboardingUrl = await _connectedAccountService.GenerateOnboardingLinkAsync(
//                connectedAccount.Id,
//                BuildOnboardingReturnUrl(connectedAccount),
//                BuildOnboardingRefreshUrl(connectedAccount));

//            return Ok(new ConnectedAccountOnboardingLinkResponseDto
//            {
//                ConnectedAccountId = connectedAccount.Id,
//                ApplicationUserId = connectedAccount.ApplicationUserId,
//                StripeAccountId = connectedAccount.StripeAccountId,
//                OnboardingUrl = onboardingUrl
//            });
//        }

//        /// <summary>
//        /// Handles the Stripe onboarding return callback for a connected account.
//        /// </summary>
//        /// <response code="200">Returns whether onboarding was completed successfully.</response>
//        /// <response code="403">If the onboarding callback token is invalid.</response>
//        [HttpGet("{id:guid}/onboarding/return")]
//        [ProducesResponseType(typeof(MessageResponseDto), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        public async Task<IActionResult> HandleOnboardingReturn(Guid id, [FromQuery] string token)
//        {
//            var isAuthorized = await ValidateOnboardingCallbackAsync(id, token);
//            if (!isAuthorized)
//            {
//                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Invalid onboarding callback token." });
//            }

//            var isComplete = await _connectedAccountService.VerifyOnboardingCompletionAsync(id);
//            if (isComplete)
//            {
//                return Ok(new MessageResponseDto { Message = "Onboarding successfully completed! You are now ready to receive payouts." });
//            }

//            return Ok(new MessageResponseDto { Message = "Onboarding was not completed fully. You may need to provide more information later." });
//        }

//        /// <summary>
//        /// Refreshes the Stripe onboarding link for a connected account callback flow.
//        /// </summary>
//        /// <response code="302">Redirects to a fresh Stripe onboarding link.</response>
//        /// <response code="403">If the onboarding callback token is invalid.</response>
//        /// <response code="404">If the connected account is not found.</response>
//        [HttpGet("{id:guid}/onboarding/refresh")]
//        [ProducesResponseType(StatusCodes.Status302Found)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> RefreshOnboardingLink(Guid id, [FromQuery] string token)
//        {
//            var isAuthorized = await ValidateOnboardingCallbackAsync(id, token);
//            if (!isAuthorized)
//            {
//                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Invalid onboarding callback token." });
//            }

//            var connectedAccount = await _connectedAccountService.GetConnectedAccountByIdAsync(id);
//            if (connectedAccount is null)
//            {
//                return NotFound(new { message = "Connected account not found" });
//            }

//            var newUrl = await _connectedAccountService.GenerateOnboardingLinkAsync(
//                id,
//                BuildOnboardingReturnUrl(connectedAccount),
//                BuildOnboardingRefreshUrl(connectedAccount));

//            return Redirect(newUrl);
//        }

//        private string BuildOnboardingReturnUrl(ConnectedAccount connectedAccount)
//        {
//            var domain = $"{Request.Scheme}://{Request.Host}";
//            var token = CreateOnboardingCallbackToken(connectedAccount);
//            return $"{domain}/api/connected-accounts/{connectedAccount.Id}/onboarding/return?token={Uri.EscapeDataString(token)}";
//        }

//        private string BuildOnboardingRefreshUrl(ConnectedAccount connectedAccount)
//        {
//            var domain = $"{Request.Scheme}://{Request.Host}";
//            var token = CreateOnboardingCallbackToken(connectedAccount);
//            return $"{domain}/api/connected-accounts/{connectedAccount.Id}/onboarding/refresh?token={Uri.EscapeDataString(token)}";
//        }

//        private async Task<bool> ValidateOnboardingCallbackAsync(Guid connectedAccountId, string token)
//        {
//            if (string.IsNullOrWhiteSpace(token))
//            {
//                return false;
//            }

//            var connectedAccount = await _connectedAccountService.GetConnectedAccountByIdAsync(connectedAccountId);
//            if (connectedAccount is null)
//            {
//                return false;
//            }

//            if (!TryReadOnboardingCallbackToken(token, out var tokenConnectedAccountId, out var tokenApplicationUserId, out var expiresAtUtc))
//            {
//                return false;
//            }

//            if (expiresAtUtc < DateTime.UtcNow)
//            {
//                return false;
//            }

//            return tokenConnectedAccountId == connectedAccountId &&
//                   tokenApplicationUserId == connectedAccount.ApplicationUserId;
//        }

//        private string CreateOnboardingCallbackToken(ConnectedAccount connectedAccount)
//        {
//            var expiresAtUtc = DateTime.UtcNow.AddMinutes(30).Ticks;
//            var payload = $"{connectedAccount.Id:N}|{connectedAccount.ApplicationUserId:N}|{expiresAtUtc}";
//            var signature = ComputeSignature(payload);

//            return $"{ToBase64Url(payload)}.{ToBase64Url(signature)}";
//        }

//        private bool TryReadOnboardingCallbackToken(string token, out Guid connectedAccountId, out Guid applicationUserId, out DateTime expiresAtUtc)
//        {
//            connectedAccountId = Guid.Empty;
//            applicationUserId = Guid.Empty;
//            expiresAtUtc = DateTime.MinValue;

//            var parts = token.Split('.', 2);
//            if (parts.Length != 2)
//            {
//                return false;
//            }

//            var payload = FromBase64Url(parts[0]);
//            var signature = FromBase64Url(parts[1]);
//            if (string.IsNullOrWhiteSpace(payload) || string.IsNullOrWhiteSpace(signature))
//            {
//                return false;
//            }

//            var expectedSignature = ComputeSignature(payload);
//            if (!CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(signature), Encoding.UTF8.GetBytes(expectedSignature)))
//            {
//                return false;
//            }

//            var payloadParts = payload.Split('|');
//            if (payloadParts.Length != 3)
//            {
//                return false;
//            }

//            if (!Guid.TryParseExact(payloadParts[0], "N", out connectedAccountId) ||
//                !Guid.TryParseExact(payloadParts[1], "N", out applicationUserId) ||
//                !long.TryParse(payloadParts[2], out var ticks))
//            {
//                return false;
//            }

//            expiresAtUtc = new DateTime(ticks, DateTimeKind.Utc);
//            return true;
//        }

//        private string ComputeSignature(string payload)
//        {
//            var secret = _configuration["Jwt:SecretKey"];
//            if (string.IsNullOrWhiteSpace(secret))
//            {
//                throw new InvalidOperationException("Jwt:SecretKey is not configured.");
//            }

//            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
//            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
//            return Convert.ToBase64String(hash);
//        }

//        private static string ToBase64Url(string value)
//        {
//            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value))
//                .TrimEnd('=')
//                .Replace('+', '-')
//                .Replace('/', '_');
//        }

//        private static string FromBase64Url(string value)
//        {
//            try
//            {
//                var base64 = value
//                    .Replace('-', '+')
//                    .Replace('_', '/');

//                switch (base64.Length % 4)
//                {
//                    case 2:
//                        base64 += "==";
//                        break;
//                    case 3:
//                        base64 += "=";
//                        break;
//                }

//                return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
//            }
//            catch
//            {
//                return string.Empty;
//            }
//        }

//        private static ConnectedAccountResponseDto MapResponse(ConnectedAccount connectedAccount)
//        {
//            return new ConnectedAccountResponseDto
//            {
//                ConnectedAccountId = connectedAccount.Id,
//                ApplicationUserId = connectedAccount.ApplicationUserId,
//                StripeAccountId = connectedAccount.StripeAccountId,
//                IsOnboardingComplete = connectedAccount.IsOnboardingComplete,
//                CreatedAt = connectedAccount.CreatedAt,
//                UpdatedAt = connectedAccount.UpdatedAt,
//                FirstName = connectedAccount.ApplicationUser?.FirstName,
//                LastName = connectedAccount.ApplicationUser?.LastName,
//                Email = connectedAccount.ApplicationUser?.Email
//            };
//        }
//    }
//}
