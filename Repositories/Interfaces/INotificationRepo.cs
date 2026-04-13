using MARN_API.DTOs.Dashboard;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface INotificationRepo
    {
        #region Dashboards
        public Task<List<NotificationMiniCardDto>> GetRenterDashboardNotifications(Guid userId);
        public Task<List<NotificationMiniCardDto>> GetOwnerDashboardNotifications(Guid userId);
        #endregion


        #region FCM Device Tokens
        Task<List<string>> GetUserDeviceTokensAsync(string userId);
        Task AddOrUpdateUserDeviceAsync(string userId, string fcmToken);
        Task RemoveUserDeviceAsync(string userId, string fcmToken);
        #endregion
    }
}
