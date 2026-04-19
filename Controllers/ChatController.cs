using MARN_API.DTOs.Chat;
using MARN_API.Models;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MARN_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : BaseController
    {
        private readonly IChatService _chatService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(
            IChatService chatService, 
            INotificationService notificationService,
            ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
            _notificationService = notificationService;
        }


        /// <summary>
        /// Retrieves a list of active users that the current user has chatted with, along with their online status and unread message count.
        /// </summary>
        /// <response code="200">
        /// Returns a list of chat-active users with their online status and unread message count. Each user object includes:
        /// - Id: The unique identifier of the user.
        /// - UserName: The full name of the user.
        /// - ProfileImage: The URL of the user's profile image.
        /// - UnreadCount: The number of unread messages from that user.
        /// - IsOnline: A boolean indicating whether the user is currently online.
        /// - LastMessage: An object containing the content and timestamp of the last message exchanged with that user.
        /// </response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="429">If rate limit is exceeded</response>
        [HttpGet("users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> GetChatUsers()
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
        /// <param name="limit">The maximum number of results to return.</param>
        /// <response code="200">
        /// Returns a list of matching users with their online status and unread message count. Each user object includes:
        /// - Id: The unique identifier of the user.
        /// - UserName: The full name of the user.
        /// - ProfileImage: The URL of the user's profile image.
        /// - UnreadCount: The number of unread messages from that user.
        /// - IsOnline: A boolean indicating whether the user is currently online.
        /// - LastMessage: An object containing the content and timestamp of the last message exchanged with that user.
        /// </response>
        /// <response code="400">If the query is empty.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="429">If rate limit is exceeded</response>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> SearchUsers([FromQuery] string q, [FromQuery] int limit = 10)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Empty search query");

            if (!TryGetUserId(out var userId))
                return Unauthorized("User ID not found in token");

            _logger.LogInformation("User {UserId} searching for users with query: {Query}", userId, q);

            var result = await _chatService.SearchUsersWithStatusAsync(userId.ToString(), q, limit);
            return HandleServiceResult(result);
        }


        /// <summary>
        /// Retrieves the chat history between the current user and another specified user.
        /// </summary>
        /// <param name="otherUserId">The ID of the other user.</param>
        /// <response code="200">
        /// Returns the chat history (list of messages) exchanged between the two users, each message with:
        /// - Id, SenderId, ReceiverId, Content, SentAt, IsRead
        /// </response>
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
    }
}
