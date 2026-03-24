using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MARN_API.DTOs.Chat;
using MARN_API.Hubs;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace MARN_API.Services.Implementations
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _repository;
        private readonly ConnectionTracker _tracker;
        private readonly IEncryptionService _encryptionService;
        private readonly IFirebaseNotificationService _fcmService;
        private readonly ILogger<ChatService> _logger;

        public ChatService(
            IChatRepository repository, 
            ConnectionTracker tracker, 
            IEncryptionService encryptionService, 
            IFirebaseNotificationService fcmService,
            ILogger<ChatService> logger)
        {
            _repository = repository;
            _tracker = tracker;
            _encryptionService = encryptionService;
            _fcmService = fcmService;
            _logger = logger;
        }

        public async Task<ServiceResult<MessageDto>> SendMessageAsync(string senderId, string receiverId, string content)
        {
            _logger.LogInformation("Sending message from {SenderId} to {ReceiverId}", senderId, receiverId);
            
            var senderUser = await _repository.GetUserByIdAsync(senderId);
            var receiverUser = await _repository.GetUserByIdAsync(receiverId);

            if (receiverUser == null)
            {
                _logger.LogWarning("Receiver user {ReceiverId} not found", receiverId);
                return ServiceResult<MessageDto>.Fail("Receiver user not found");
            }

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
                if (receiverTokens != null && receiverTokens.Any())
                {
                    var senderNameStr = senderUser?.UserName ?? "Someone";
                    _logger.LogInformation("Receiver {ReceiverId} is offline, sending FCM notification", receiverId);
                    await _fcmService.SendNotificationAsync(receiverTokens, $"New Message from {senderNameStr}", content);
                }
            }

            var dto = new MessageDto
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

            return ServiceResult<MessageDto>.Ok(dto);
        }

        public async Task<ServiceResult<List<MessageDto>>> GetChatHistoryAsync(string currentUserId, string otherUserId)
        {
            _logger.LogInformation("Fetching chat history between {UserId} and {OtherUserId}", currentUserId, otherUserId);
            
            var messages = await _repository.GetMessagesBetweenUsersAsync(currentUserId, otherUserId);
            
            // Mark any unread messages from this user to me as read
            await _repository.MarkMessagesAsReadAsync(senderId: otherUserId, receiverId: currentUserId);

            var result = messages.Select(m => new MessageDto
            {
                Id = m.Id,
                SenderId = m.SenderId.ToString(),
                ReceiverId = m.ReceiverId.ToString(),
                Content = _encryptionService.Decrypt(m.Content), // Decrypt for the UI
                SentAt = m.SentAt,
                IsRead = m.IsRead 
            }).ToList();

            return ServiceResult<List<MessageDto>>.Ok(result);
        }

        public async Task<ServiceResult<bool>> MarkChatAsReadAsync(string currentUserId, string senderId)
        {
            _logger.LogInformation("Marking messages from {SenderId} to {ReceiverId} as read", senderId, currentUserId);
            await _repository.MarkMessagesAsReadAsync(senderId: senderId, receiverId: currentUserId);
            return ServiceResult<bool>.Ok(true);
        }

        public async Task<ServiceResult<List<UserDto>>> GetActiveUsersWithStatusAsync(string currentUserId)
        {
            _logger.LogInformation("Fetching active chat users for {UserId}", currentUserId);
            var usersData = await _repository.GetActiveChatUsersWithUnreadCountAsync(currentUserId);
            
            var result = usersData.Select(data => new UserDto
            {
                Id = data.User.Id.ToString(),
                UserName = data.User.UserName,
                Email = data.User.Email,
                UnreadCount = data.UnreadCount,
                IsOnline = _tracker.IsOnline(data.User.Id.ToString())
            }).ToList();

            return ServiceResult<List<UserDto>>.Ok(result);
        }

        public async Task<ServiceResult<List<UserDto>>> SearchUsersWithStatusAsync(string currentUserId, string query)
        {
            _logger.LogInformation("Searching for users with query '{Query}' for {UserId}", query, currentUserId);
            var users = await _repository.SearchUsersAsync(currentUserId, query, 20);
            
            var result = users.Select(u => new UserDto
            {
                Id = u.Id.ToString(),
                UserName = u.UserName,
                Email = u.Email,
                UnreadCount = 0, // Assume 0 for search results 
                IsOnline = _tracker.IsOnline(u.Id.ToString())
            }).ToList();

            return ServiceResult<List<UserDto>>.Ok(result);
        }

        public async Task<ServiceResult<bool>> SaveDeviceTokenAsync(string userId, string fcmToken)
        {
            _logger.LogInformation("Saving FCM token for user {UserId}", userId);
            await _repository.AddOrUpdateUserDeviceAsync(userId, fcmToken);
            return ServiceResult<bool>.Ok(true);
        }
    }
}
