using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MARN_API.DTOs.Chat;
using MARN_API.DTOs.Notification;
using MARN_API.Enums.Notification;
using MARN_API.Hubs;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MARN_API.Services.Implementations
{
    public class ChatService : IChatService
    {
        private readonly IChatRepo _chatRepo;
        private readonly INotificationRepo _notificationRepo;
        private readonly ConnectionTracker _tracker;
        private readonly IEncryptionService _encryptionService;
        private readonly IFirebaseNotificationService _fcmService;
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ChatService> _logger;

        public ChatService(
            IChatRepo chatRepo, 
            INotificationRepo notificationRepo,
            ConnectionTracker tracker, 
            IEncryptionService encryptionService, 
            IFirebaseNotificationService fcmService,
            INotificationService notificationService,
            UserManager<ApplicationUser> userManager,
            ILogger<ChatService> logger)
        {
            _chatRepo = chatRepo;
            _notificationRepo = notificationRepo;
            _tracker = tracker;
            _encryptionService = encryptionService;
            _fcmService = fcmService;
            _notificationService = notificationService;
            _userManager = userManager;
            _logger = logger;
        }


        #region Chats Page
        public async Task<ServiceResult<List<ChatUserDto>>> GetActiveUsersWithStatusAsync(string currentUserId)
        {
            _logger.LogInformation("Fetching active chat users for {UserId}", currentUserId);
            var result = await _chatRepo.GetActiveChatUsersWithUnreadCountAsync(currentUserId);

            foreach (var user in result)
            { 
                user.IsOnline = _tracker.IsOnline(user.Id);
                if (user.LastMessage != null)
                    user.LastMessage.Content = _encryptionService.Decrypt(user.LastMessage.Content);
            }

            return ServiceResult<List<ChatUserDto>>.Ok(result);
        }

        public async Task<ServiceResult<List<ChatUserDto>>> SearchUsersWithStatusAsync(string currentUserId, string query, int limit)
        {
            _logger.LogInformation("Searching for users with query '{Query}' for {UserId}", query, currentUserId);
            var result = await _chatRepo.SearchUsersAsync(currentUserId, query, limit);

            foreach (var user in result)
            {
                user.IsOnline = _tracker.IsOnline(user.Id);
                if (user.LastMessage != null)
                    user.LastMessage.Content = _encryptionService.Decrypt(user.LastMessage.Content);
            }

            return ServiceResult<List<ChatUserDto>>.Ok(result);
        }

        public async Task<ServiceResult<List<MessageDto>>> GetChatHistoryAsync(string currentUserId, string otherUserId)
        {
            _logger.LogInformation("Fetching chat history between {UserId} and {OtherUserId}", currentUserId, otherUserId);
            
            var messages = await _chatRepo.GetMessagesBetweenUsersAsync(currentUserId, otherUserId);

            var result = messages.Select(m => new MessageDto
            {
                Id = m.Id,
                SenderId = m.SenderId.ToString(),
                ReceiverId = m.ReceiverId.ToString(),
                Content = _encryptionService.Decrypt(m.Content), // Decrypt for the UI
                SentAt = m.SentAt,
                IsRead = m.ReadAt.HasValue
            }).ToList();

            return ServiceResult<List<MessageDto>>.Ok(result);
        }
        #endregion

        
        #region Messages Page
        public async Task<ServiceResult<MessageDto>> SendMessageAsync(string senderId, string receiverId, string content)
        {
            _logger.LogInformation("Sending message from {SenderId} to {ReceiverId}", senderId, receiverId);

            // 1. Check the input
            if (!Guid.TryParse(senderId, out var senderGuid) ||
                !Guid.TryParse(receiverId, out var receiverGuid))
            {
                return ServiceResult<MessageDto>.Fail("Invalid userId format");
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                return ServiceResult<MessageDto>.Fail("Message content cannot be empty");
            }

            var senderUser = await _userManager.FindByIdAsync(senderId);
            if (senderUser == null)
            {
                _logger.LogWarning("Sender user {SenderId} not found", senderId);
                return ServiceResult<MessageDto>.Fail("Sender user not found");
            }

            var receiverUser = await _userManager.FindByIdAsync(receiverId);
            if (receiverUser == null)
            {
                _logger.LogWarning("Receiver user {ReceiverId} not found", receiverId);
                return ServiceResult<MessageDto>.Fail("Receiver user not found");
            }


            // 2. Save The Message
            var message = new Message
            {
                Id = Guid.NewGuid(),
                SenderId = senderGuid,
                ReceiverId = receiverGuid,
                Content = _encryptionService.Encrypt(content),
                SentAt = DateTime.UtcNow,
            };

            await _chatRepo.AddMessageAsync(message);


            // 3. Send Notification
            await _notificationService.SendNotificationAsync( new NotificationRequestDto
            {
                ReceiverId = receiverId,
                SenderId = senderId,
                Type = NotificationType.NewMessage,
                Title = "New Message",
                Body = $"You have a new message from {senderUser.FirstName} {senderUser.LastName}",
                Data = new Dictionary<string, string>
                {
                    { "SenderId", senderId },
                    { "SenderName", $"{senderUser.FirstName} {senderUser.LastName}" },
                    { "Content", content }
                }
            });


            // 4. Return The Message
            var dto = new MessageDto
            {
                Id = message.Id,
                SenderId = message.SenderId.ToString(),
                SenderName = $"{senderUser.FirstName} {senderUser.LastName}",
                ReceiverId = message.ReceiverId.ToString(),
                ReceiverName = $"{receiverUser.FirstName} {receiverUser.LastName}",
                Content = content, // Return plaintext to the sender's UI
                SentAt = message.SentAt,
                IsRead = message.ReadAt.HasValue
            };

            return ServiceResult<MessageDto>.Ok(dto);
        }

        public async Task<ServiceResult<bool>> MarkChatAsReadAsync(string currentUserId, string senderId)
        {
            _logger.LogInformation("Marking messages from {SenderId} to {ReceiverId} as read", senderId, currentUserId);

            await _chatRepo.MarkMessagesAsReadAsync(senderId: senderId, receiverId: currentUserId);
            return ServiceResult<bool>.Ok(true);
        }
        #endregion
    }
}
