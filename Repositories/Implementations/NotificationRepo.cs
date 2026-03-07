using MARN_API.Data;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class NotificationRepo : INotificationRepo
    {
        private readonly AppDbContext Context;
        public NotificationRepo(AppDbContext context)
        {
            Context = context;
        }

        public Task<List<Notification>> GetNotifications(Guid userId)
        {
            return Context.Notifications
                .Where(n => n.UserId == userId)
                .ToListAsync();
        }
    }
}
