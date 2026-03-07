using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface INotificationRepo
    {
        public Task<List<Notification>> GetNotifications(Guid userId);
    }
}
