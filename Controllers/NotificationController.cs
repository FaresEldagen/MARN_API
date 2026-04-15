using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MARN_API.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MARN_API.DTOs.Notification;
using Microsoft.AspNetCore.Http.HttpResults;
using MARN_API.Enums.Notification;

namespace MARN_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(
            INotificationService notificationService,
            ILogger<NotificationController> logger)
        {
            _logger = logger;
            _notificationService = notificationService;
        }


        /// <summary>
        /// Saves or updates the Firebase Cloud Messaging (FCM) token for the current user's device.
        /// </summary>
        /// <param name="request">The request containing the FCM token.</param>
        /// <response code="200">FCM token saved successfully.</response>
        /// <response code="400">If the token is missing.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpPost("save-fcm-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SaveFcmToken([FromBody] FcmTokenRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.token))
                return BadRequest("Token is required.");

            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            _logger.LogInformation("User {UserId} saving FCM token", userId);

            var result = await _notificationService.SaveDeviceTokenAsync(userId.ToString(), request.token);
            return HandleServiceResult(result);
        }


        /// <summary>
        /// Removes the Firebase Cloud Messaging (FCM) token for the current user's device.
        /// </summary>
        /// <param name="request">The request containing the FCM token.</param>
        /// <response code="200">FCM token removed successfully.</response>
        /// <response code="400">If the token is missing.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpDelete("remove-fcm-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RemoveFcmToken([FromBody] FcmTokenRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.token))
                return BadRequest("Token is required.");

            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            _logger.LogInformation("User {UserId} removing FCM token", userId);

            var result = await _notificationService.RemoveDeviceTokenAsync(userId.ToString(), request.token);
            return HandleServiceResult(result);
        }


        [HttpGet("test-notification")]
        public async Task<IActionResult> TestNotification()
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                ReceiverId = userId.ToString(),
                Type = NotificationType.None,
                Title = "Test Notificaiton",
                Body = $"This is a test notification",
                SaveInDB = false
            });

            return Ok();
        }
    }
}
