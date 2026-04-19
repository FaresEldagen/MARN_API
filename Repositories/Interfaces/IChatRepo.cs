using System.Collections.Generic;
using System.Threading.Tasks;
using MARN_API.DTOs.Chat;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IChatRepo
    {
        #region Chats Page
        Task<List<ChatUserDto>> GetActiveChatUsersWithUnreadCountAsync(string currentUserId);
        Task<List<ChatUserDto>> SearchUsersAsync(string currentUserId, string query, int limit);
        Task<List<Message>> GetMessagesBetweenUsersAsync(string userId1, string userId2);
        #endregion


        #region Messages Page
        Task<Message> AddMessageAsync(Message message);
        Task MarkMessagesAsReadAsync(string senderId, string receiverId);
        #endregion
    }
}
