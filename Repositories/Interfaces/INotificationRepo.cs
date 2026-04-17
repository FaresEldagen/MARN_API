using MARN_API.DTOs.Dashboard;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface INotificationRepo
    {
        #region Notification
        public Task AddAsync(Notification notification);

        public Task<List<Notification>> GetAllNotificationsAsync(string user);
        public Task<List<NotificationMiniCardDto>> GetRenterDashboardNotifications(Guid userId);
        public Task<List<NotificationMiniCardDto>> GetOwnerDashboardNotifications(Guid userId);

        public Task MarkAllAsReadAsync(string userId);
        public Task MarkAsReadAsync(string userId, long notificationId);
        #endregion


        #region FCM Device Tokens
        Task<List<string>> GetUserDeviceTokensAsync(string userId);
        Task AddOrUpdateUserDeviceAsync(string userId, string fcmToken);
        Task RemoveUserDeviceAsync(string userId, string fcmToken);
        #endregion
    }
}
