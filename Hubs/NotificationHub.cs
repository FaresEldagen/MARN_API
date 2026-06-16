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
        private readonly ConnectionTracker _tracker;
        public NotificationHub(INotificationService notificationService, ConnectionTracker tracker)
        {
            _notificationService = notificationService;
            _tracker = tracker;
        }


        //public override async Task OnConnectedAsync()
        //{
        //    var userId = Context.UserIdentifier;
        //    if (userId != null)
        //        _tracker.UserConnected(userId);

        //    await base.OnConnectedAsync();
        //}

        //public override async Task OnDisconnectedAsync(Exception? exception)
        //{
        //    var userId = Context.UserIdentifier;
        //    if (userId != null)
        //        _tracker.UserDisconnected(userId);

        //    await base.OnDisconnectedAsync(exception);
        //}


        public async Task MarkAllNotificationsAsRead()
        {
            var currentUserId = Context.UserIdentifier;
            if (string.IsNullOrEmpty(currentUserId)) return;

            await _notificationService.MarkAllNotificationsAsReadAsync(currentUserId);
            await Clients.User(currentUserId.ToLower()).SendAsync("AllNotificationsMarkedAsRead");

        }

        public async Task MarkNotificationAsRead(long notificationId)
        {
            var currentUserId = Context.UserIdentifier;
            if (string.IsNullOrEmpty(currentUserId)) return;

            await _notificationService.MarkNotificationAsReadAsync(currentUserId, notificationId);
            await Clients.User(currentUserId.ToLower()).SendAsync("NotificationMarkedAsRead", notificationId);

        }
    }
}
