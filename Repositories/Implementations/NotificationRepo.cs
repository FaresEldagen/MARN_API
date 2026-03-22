using MARN_API.Data;
using MARN_API.DTOs.Dashboard;
using MARN_API.Enums;
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


        #region Dashboards
        public Task<List<NotificationMiniCardDto>> GetRenterDashboardNotifications(Guid userId)
        {
            return Context.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == userId && n.UserType == NotificationUserTypeEnum.Renter)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationMiniCardDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    IsRead = n.ReadAt.HasValue,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync();
        }

        public Task<List<NotificationMiniCardDto>> GetOwnerDashboardNotifications(Guid userId)
        {
            return Context.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == userId && n.UserType == NotificationUserTypeEnum.Owner)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationMiniCardDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    IsRead = n.ReadAt.HasValue,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync();
        }
        #endregion
    }
}
