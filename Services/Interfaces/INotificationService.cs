using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface INotificationService
    {
        Task<ServiceResult<bool>> SaveDeviceTokenAsync(string userId, string fcmToken);
        Task<ServiceResult<bool>> RemoveDeviceTokenAsync(string userId, string fcmToken);
        Task SendMessageNotificationAsync(string receiverId, string senderName, string content);

    }
}
