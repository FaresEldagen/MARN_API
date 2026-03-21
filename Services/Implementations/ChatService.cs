using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MARN_API.DTOs.Chat;
using MARN_API.Hubs;
using MARN_API.Models;
using MARN_API.Repositories;

namespace MARN_API.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _repository;
        private readonly ConnectionTracker _tracker;
        private readonly IEncryptionService _encryptionService;
        private readonly IFirebaseNotificationService _fcmService;

        public ChatService(IChatRepository repository, ConnectionTracker tracker, IEncryptionService encryptionService, IFirebaseNotificationService fcmService)
        {
            _repository = repository;
            _tracker = tracker;
            _encryptionService = encryptionService;
            _fcmService = fcmService;
        }

        public async Task<MessageDto> SendMessageAsync(string senderId, string receiverId, string content)
        {
            var senderUser = await _repository.GetUserByIdAsync(senderId);
            var receiverUser = await _repository.GetUserByIdAsync(receiverId);

            var message = new Message
            {
                Id = Guid.NewGuid(),
                SenderId = Guid.Parse(senderId),
                ReceiverId = Guid.Parse(receiverId),
                Content = _encryptionService.Encrypt(content), // Encrypt before saving to DB
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            await _repository.AddMessageAsync(message);

            // Trigger FCM Notification if the receiver is Offline (meaning their SignalR WebSocket is dead)
            if (!_tracker.IsOnline(receiverId))
            {
                var receiverTokens = await _repository.GetUserDeviceTokensAsync(receiverId);
                if (receiverTokens.Any())
                {
                    var senderNameStr = senderUser?.UserName ?? "Someone";
                    await _fcmService.SendNotificationAsync(receiverTokens, $"New Message from {senderNameStr}", content);
                }
            }

            return new MessageDto
            {
                Id = message.Id,
                SenderId = message.SenderId.ToString(),
                SenderName = senderUser?.UserName,
                ReceiverId = message.ReceiverId.ToString(),
                ReceiverName = receiverUser?.UserName,
                Content = content, // Return plaintext to the sender's UI
                SentAt = message.SentAt,
                IsRead = message.IsRead
            };
        }

        public async Task<List<MessageDto>> GetChatHistoryAsync(string currentUserId, string otherUserId)
        {
            var messages = await _repository.GetMessagesBetweenUsersAsync(currentUserId, otherUserId);
            
            // Mark any unread messages from this user to me as read
            await _repository.MarkMessagesAsReadAsync(senderId: otherUserId, receiverId: currentUserId);

            return messages.Select(m => new MessageDto
            {
                Id = m.Id,
                SenderId = m.SenderId.ToString(),
                ReceiverId = m.ReceiverId.ToString(),
                Content = _encryptionService.Decrypt(m.Content), // Decrypt for the UI
                SentAt = m.SentAt,
                IsRead = m.IsRead 
            }).ToList();
        }

        public async Task MarkChatAsReadAsync(string currentUserId, string senderId)
        {
            await _repository.MarkMessagesAsReadAsync(senderId: senderId, receiverId: currentUserId);
        }

        public async Task<List<UserDto>> GetActiveUsersWithStatusAsync(string currentUserId)
        {
            var usersData = await _repository.GetActiveChatUsersWithUnreadCountAsync(currentUserId);
            
            return usersData.Select(data => new UserDto
            {
                Id = data.User.Id.ToString(),
                UserName = data.User.UserName,
                Email = data.User.Email,
                UnreadCount = data.UnreadCount,
                IsOnline = _tracker.IsOnline(data.User.Id.ToString())
            }).ToList();
        }

        public async Task<List<UserDto>> SearchUsersWithStatusAsync(string currentUserId, string query)
        {
            var users = await _repository.SearchUsersAsync(currentUserId, query, 20);
            
            return users.Select(u => new UserDto
            {
                Id = u.Id.ToString(),
                UserName = u.UserName,
                Email = u.Email,
                UnreadCount = 0, // Assume 0 for search results 
                IsOnline = _tracker.IsOnline(u.Id.ToString())
            }).ToList();
        }

        public async Task SaveDeviceTokenAsync(string userId, string fcmToken)
        {
            await _repository.AddOrUpdateUserDeviceAsync(userId, fcmToken);
        }
    }
}
