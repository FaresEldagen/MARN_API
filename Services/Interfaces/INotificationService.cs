using MARN_API.DTOs.Notification;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface INotificationService
    {
        Task<ServiceResult<bool>> SaveDeviceTokenAsync(string userId, string fcmToken);
        Task<ServiceResult<bool>> RemoveDeviceTokenAsync(string userId, string fcmToken);
        Task SendNotificationAsync(NotificationRequestDto request);
    }
}
