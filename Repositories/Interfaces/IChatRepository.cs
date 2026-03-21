using System.Collections.Generic;
using System.Threading.Tasks;
using MARN_API.Models;

namespace MARN_API.Repositories
{
    public interface IChatRepository
    {
        Task<Message> AddMessageAsync(Message message);
        Task<List<Message>> GetMessagesBetweenUsersAsync(string userId1, string userId2);
        Task MarkMessagesAsReadAsync(string senderId, string receiverId);
        
        Task<List<(ApplicationUser User, int UnreadCount)>> GetActiveChatUsersWithUnreadCountAsync(string currentUserId);
        Task<List<ApplicationUser>> SearchUsersAsync(string currentUserId, string query, int limit);
        Task<ApplicationUser?> GetUserByIdAsync(string userId);

        Task AddOrUpdateUserDeviceAsync(string userId, string fcmToken);
        Task<List<string>> GetUserDeviceTokensAsync(string userId);
    }
}
