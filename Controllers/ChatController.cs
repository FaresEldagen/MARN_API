using MARN_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MARN_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // GET: api/chat/users
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

            var users = await _chatService.GetActiveUsersWithStatusAsync(currentUserId);
            return Ok(users);
        }

        // GET: api/chat/search?q={query}
        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Empty search query");

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

            var users = await _chatService.SearchUsersWithStatusAsync(currentUserId, q);
            return Ok(users);
        }

        // GET: api/chat/history/{otherUserId}
        // Returns the chat history between the current logged-in user and the specified user
        [HttpGet("history/{otherUserId}")]
        public async Task<IActionResult> GetChatHistory(string otherUserId)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId) || string.IsNullOrEmpty(otherUserId))
                return BadRequest("Invalid user IDs.");
            
            var messages = await _chatService.GetChatHistoryAsync(currentUserId, otherUserId);
            return Ok(messages);
        }

        // POST: api/chat/fcm-token
        public class FcmTokenRequest { public string Token { get; set; } = string.Empty; }

        [HttpPost("fcm-token")]
        public async Task<IActionResult> SaveFcmToken([FromBody] FcmTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Token)) return BadRequest("Token is required.");

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

            await _chatService.SaveDeviceTokenAsync(currentUserId, request.Token);
            return Ok();
        }
    }
}
