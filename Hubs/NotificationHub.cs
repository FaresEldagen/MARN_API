using MARN_API.Services.Implementations;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace MARN_API.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly INotificationService _notificationService;
        public NotificationHub(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        public async Task MarkAllNotificationsAsRead()
        {
            var currentUserId = Context.UserIdentifier;
            if (string.IsNullOrEmpty(currentUserId)) return;

            await _notificationService.MarkAllNotificationsAsReadAsync(currentUserId);
            await Clients.Caller.SendAsync("AllNotificationsMarkedAsRead");

        }

        public async Task MarkNotificationAsRead(long notificationId)
        {
            var currentUserId = Context.UserIdentifier;
            if (string.IsNullOrEmpty(currentUserId)) return;

            await _notificationService.MarkNotificationAsReadAsync(currentUserId, notificationId);
            await Clients.Caller.SendAsync("NotificationMarkedAsRead", notificationId);

        }
    }
}
