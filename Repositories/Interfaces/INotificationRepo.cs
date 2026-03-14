using MARN_API.DTOs.Dashboard;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface INotificationRepo
    {
        public Task<List<NotificationCardDto>> GetNotifications(Guid userId);
    }
}
