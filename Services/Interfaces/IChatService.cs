using System.Collections.Generic;
using System.Threading.Tasks;
using MARN_API.DTOs.Chat;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IChatService
    {
        #region Chats Page
        Task<ServiceResult<List<ChatUserDto>>> GetActiveUsersWithStatusAsync(string currentUserId);
        Task<ServiceResult<List<ChatUserDto>>> SearchUsersWithStatusAsync(string currentUserId, string query, int limit);
        Task<ServiceResult<List<MessageDto>>> GetChatHistoryAsync(string currentUserId, string otherUserId);
        #endregion


        #region Messages Page
        Task<ServiceResult<MessageDto>> SendMessageAsync(string senderId, string receiverId, string content);
        Task<ServiceResult<bool>> MarkChatAsReadAsync(string currentUserId, string senderId);
        #endregion
    }
}
