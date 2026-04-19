using MARN_API.DTOs.Notification;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface INotificationService
    {
        #region Notification
        public Task SendNotificationAsync(NotificationRequestDto request);

        public Task<ServiceResult<List<NotificationCardDto>>> GetUserNotificationsAsync(Guid userId);

        public Task MarkAllNotificationsAsReadAsync(string currentUserId);
        public Task MarkNotificationAsReadAsync(string currentUserId, long notificationId);
        #endregion


        #region FCM Device Tokens
        public Task<ServiceResult<bool>> SaveDeviceTokenAsync(string userId, string fcmToken);
        public Task<ServiceResult<bool>> RemoveDeviceTokenAsync(string userId, string fcmToken);
        #endregion
    }
}
