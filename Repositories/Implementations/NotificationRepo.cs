using MARN_API.Data;
using MARN_API.DTOs.Dashboard;
using MARN_API.Enums.Notification;
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


        #region Notification
        public async Task AddAsync(Notification notification)
        {
            Context.Notifications.Add(notification);
            await Context.SaveChangesAsync();
        }


        public async Task<List<Notification>> GetAllNotificationsAsync(string userId)
        {
            return await Context.Notifications
                .Where(n => n.UserId.ToString() == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public Task<List<NotificationMiniCardDto>> GetRenterDashboardNotifications(Guid userId)
        {
            return Context.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == userId && 
                    (n.UserType == NotificationUserType.Renter || 
                    n.UserType == NotificationUserType.General))
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
                .Where(n => n.UserId == userId && 
                    (n.UserType == NotificationUserType.Owner || 
                    n.UserType == NotificationUserType.General))
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


        public async Task MarkAllAsReadAsync(string userId)
        {
            await Context.Notifications
                .Where(n => n.UserId.ToString() == userId && n.ReadAt == null)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(m => m.ReadAt, DateTime.UtcNow));
        }

        public async Task MarkAsReadAsync(string userId, long notificationId)
        {
            var notification = await Context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId.ToString() == userId);
            if (notification != null && !notification.ReadAt.HasValue)
            {
                notification.ReadAt = DateTime.UtcNow;
                await Context.SaveChangesAsync();
            }
        }


        public async Task DeleteNotificationsByUserIdAsync(Guid userId)
        {
            await Context.Notifications
                .Where(n => n.UserId == userId)
                .ExecuteDeleteAsync();
        }
        #endregion


        #region FCM Device Tokens
        public async Task<List<string>> GetUserDeviceTokensAsync(string userId)
        {
            return await Context.UserDevices
                .Where(d => d.UserId == userId)
                .Select(d => d.FcmToken)
                .ToListAsync();
        }

        public async Task AddOrUpdateUserDeviceAsync(string userId, string fcmToken)
        {
            var device = await Context.UserDevices
                .FirstOrDefaultAsync(d => d.FcmToken == fcmToken);
            if (device == null)
            {
                Context.UserDevices.Add(new UserDevice
                {
                    Id = System.Guid.NewGuid(),
                    UserId = userId,
                    FcmToken = fcmToken,
                    LastUpdated = System.DateTime.UtcNow
                });
            }
            else
            {
                // In case a device changes hands (logout and new user logs in on same phone)
                device.UserId = userId;
                device.LastUpdated = System.DateTime.UtcNow;
            }
            await Context.SaveChangesAsync();
        }

        public async Task RemoveUserDeviceAsync(string userId, string fcmToken)
        {
            var device = await Context.UserDevices
                .FirstOrDefaultAsync(d => d.FcmToken == fcmToken && d.UserId == userId);
            if (device != null)
            {
                Context.UserDevices.Remove(device);
                await Context.SaveChangesAsync();
            }
        }

        public async Task DeleteDevicesByUserIdAsync(string userId)
        {
            await Context.UserDevices
                .Where(d => d.UserId == userId)
                .ExecuteDeleteAsync();
        }
        #endregion
    }
}
