using MARN_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace MARN_API.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly ConnectionTracker _tracker;

        public ChatHub(IChatService chatService, ConnectionTracker tracker)
        {
            _chatService = chatService;
            _tracker = tracker;
        }

        public async Task SendMessage(string receiverId, string content)
        {
            var senderId = Context.UserIdentifier;
            if (string.IsNullOrEmpty(senderId) || string.IsNullOrEmpty(receiverId) || string.IsNullOrEmpty(content))
                throw new HubException("Invalid receiver or message content.");
            
            // 1. Save message to Database via Service abstraction
            var payload = await _chatService.SendMessageAsync(senderId, receiverId, content);

            // 2. Deliver message in real-time to the Receiver (if they are online)
            await Clients.User(receiverId).SendAsync("ReceiveMessage", payload);

            // 3. Echo the message back to the sender's other devices
            await Clients.User(senderId).SendAsync("ReceiveMessage", payload);
        }

        public async Task MarkChatAsRead(string senderId)
        {
            var currentUserId = Context.UserIdentifier;
            if (string.IsNullOrEmpty(currentUserId) || string.IsNullOrEmpty(senderId)) return;

            await _chatService.MarkChatAsReadAsync(currentUserId: currentUserId, senderId: senderId);
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (userId != null)
            {
                var isNewOnline = _tracker.UserConnected(userId);
                if (isNewOnline)
                {
                    // Broadcast to everyone that this user is now online
                    await Clients.All.SendAsync("UserOnline", userId);
                }
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            if (userId != null)
            {
                var isOffline = _tracker.UserDisconnected(userId);
                if (isOffline)
                {
                    // Broadcast to everyone that this user is now offline
                    await Clients.All.SendAsync("UserOffline", userId);
                }
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
