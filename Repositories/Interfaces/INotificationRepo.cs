using MARN_API.DTOs.Dashboard;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface INotificationRepo
    {
        public Task<List<NotificationMiniCardDto>> GetRenterDashboardNotifications(Guid userId);
        public Task<List<NotificationMiniCardDto>> GetOwnerDashboardNotifications(Guid userId);
    }
}
