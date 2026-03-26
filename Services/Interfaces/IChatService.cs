using System.Collections.Generic;
using System.Threading.Tasks;
using MARN_API.DTOs.Chat;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IChatService
    {
        Task<ServiceResult<MessageDto>> SendMessageAsync(string senderId, string receiverId, string content);
        Task<ServiceResult<List<MessageDto>>> GetChatHistoryAsync(string currentUserId, string otherUserId);
        Task<ServiceResult<bool>> MarkChatAsReadAsync(string currentUserId, string senderId);
        Task<ServiceResult<List<UserDto>>> GetActiveUsersWithStatusAsync(string currentUserId);
        Task<ServiceResult<List<UserDto>>> SearchUsersWithStatusAsync(string currentUserId, string query);
        Task<ServiceResult<bool>> SaveDeviceTokenAsync(string userId, string fcmToken);
    }
}
