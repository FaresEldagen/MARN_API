using System.Collections.Generic;
using System.Threading.Tasks;
using MARN_API.DTOs.Chat;

namespace MARN_API.Services
{
    public interface IChatService
    {
        Task<MessageDto> SendMessageAsync(string senderId, string receiverId, string content);
        Task<List<MessageDto>> GetChatHistoryAsync(string currentUserId, string otherUserId);
        Task MarkChatAsReadAsync(string currentUserId, string senderId);
        Task<List<UserDto>> GetActiveUsersWithStatusAsync(string currentUserId);
        Task<List<UserDto>> SearchUsersWithStatusAsync(string currentUserId, string query);
        Task SaveDeviceTokenAsync(string userId, string fcmToken);
    }
}
