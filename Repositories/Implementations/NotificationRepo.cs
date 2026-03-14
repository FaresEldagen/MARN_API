using MARN_API.Data;
using MARN_API.DTOs.Dashboard;
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

        public Task<List<NotificationCardDto>> GetNotifications(Guid userId)
        {
            return Context.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationCardDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    IsRead = n.ReadAt.HasValue,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync();
        }
    }
}
