using MARN_API.DTOs.Chat;
using MARN_API.Models;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MARN_API.Controllers
{
    /// <summary>
    /// Controller for handling real-time chat operations.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : BaseController
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatService chatService, ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of active users that the current user has chatted with, along with their online status and unread message count.
        /// </summary>
        /// <response code="200">Returns a list of chat-active users.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet("users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUsers()
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            _logger.LogInformation("User {UserId} requested active chat users", userId);
            var result = await _chatService.GetActiveUsersWithStatusAsync(userId.ToString());
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Searches for users by username or email.
        /// </summary>
        /// <param name="q">The search query string.</param>
        /// <response code="200">Returns a list of matching users.</response>
        /// <response code="400">If the query is empty.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SearchUsers([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Empty search query");

            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            _logger.LogInformation("User {UserId} searching for users with query: {Query}", userId, q);
            var result = await _chatService.SearchUsersWithStatusAsync(userId.ToString(), q);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Retrieves the chat history between the current user and another specified user.
        /// </summary>
        /// <param name="otherUserId">The ID of the other user.</param>
        /// <response code="200">Returns the chat history (list of messages) exchanged between the two users.</response>
        /// <response code="400">If the other user ID is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet("history/{otherUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetChatHistory(string otherUserId)
        {
            if (string.IsNullOrEmpty(otherUserId))
                return BadRequest("Other User ID is required.");

            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            _logger.LogInformation("User {UserId} requested history with {OtherUserId}", userId, otherUserId);
            var result = await _chatService.GetChatHistoryAsync(userId.ToString(), otherUserId);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// DTO for FCM token registration request.
        /// </summary>
        public class FcmTokenRequest { public string Token { get; set; } = string.Empty; }

        /// <summary>
        /// Saves or updates the Firebase Cloud Messaging (FCM) token for the current user's device.
        /// </summary>
        /// <param name="request">The request containing the FCM token.</param>
        /// <response code="200">FCM token saved successfully.</response>
        /// <response code="400">If the token is missing.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpPost("fcm-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SaveFcmToken([FromBody] FcmTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Token)) 
                return BadRequest("Token is required.");

            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            _logger.LogInformation("User {UserId} saving FCM token", userId);
            var result = await _chatService.SaveDeviceTokenAsync(userId.ToString(), request.Token);
            return HandleServiceResult(result);
        }
    }
}
