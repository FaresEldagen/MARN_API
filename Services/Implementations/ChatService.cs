using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MARN_API.DTOs.Chat;
using MARN_API.DTOs.Notification;
using MARN_API.Enums;
using MARN_API.Enums.Notification;
using MARN_API.Hubs;
using MARN_API.Data;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MARN_API.Services.Implementations
{
    public class ChatService : IChatService
    {
        private const string HiddenMessagePlaceholder = "[Message hidden by admin]";
        private readonly IChatRepo _chatRepo;
        private readonly INotificationRepo _notificationRepo;
        private readonly ConnectionTracker _tracker;
        private readonly IEncryptionService _encryptionService;
        private readonly IFirebaseNotificationService _fcmService;
        private readonly INotificationService _notificationService;
        private readonly IAppTextLocalizer _localizer;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ChatService> _logger;

        public ChatService(
            IChatRepo chatRepo, 
            INotificationRepo notificationRepo,
            ConnectionTracker tracker, 
            IEncryptionService encryptionService, 
            IFirebaseNotificationService fcmService,
            INotificationService notificationService,
            IAppTextLocalizer localizer,
            UserManager<ApplicationUser> userManager,
            AppDbContext dbContext,
            ILogger<ChatService> logger)
        {
            _chatRepo = chatRepo;
            _notificationRepo = notificationRepo;
            _tracker = tracker;
            _encryptionService = encryptionService;
            _fcmService = fcmService;
            _notificationService = notificationService;
            _localizer = localizer;
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }


        #region Chats Page
        public async Task<ServiceResult<List<ChatUserDto>>> GetActiveUsersWithStatusAsync(string currentUserId)
        {
            var accessCheck = await ValidateChatAccessAsync(currentUserId);
            if (!accessCheck.Success)
                return ServiceResult<List<ChatUserDto>>.Fail(accessCheck.Message!, resultType: accessCheck.ResultType);

            _logger.LogInformation("Fetching active chat users for {UserId}", currentUserId);
            var result = await _chatRepo.GetActiveChatUsersWithUnreadCountAsync(currentUserId);

            foreach (var user in result)
            { 
                user.IsOnline = _tracker.IsOnline(user.Id);
                if (user.LastMessage != null)
                    user.LastMessage.Content = user.LastMessage.IsHiddenByModeration
                        ? T(HiddenMessagePlaceholder)
                        : _encryptionService.Decrypt(user.LastMessage.Content);
            }

            return ServiceResult<List<ChatUserDto>>.Ok(result);
        }

        public async Task<ServiceResult<List<ChatUserDto>>> SearchUsersWithStatusAsync(string currentUserId, string query, int limit)
        {
            var accessCheck = await ValidateChatAccessAsync(currentUserId);
            if (!accessCheck.Success)
                return ServiceResult<List<ChatUserDto>>.Fail(accessCheck.Message!, resultType: accessCheck.ResultType);

            _logger.LogInformation("Searching for users with query '{Query}' for {UserId}", query, currentUserId);
            var result = await _chatRepo.SearchUsersAsync(currentUserId, query, limit);

            foreach (var user in result)
            {
                user.IsOnline = _tracker.IsOnline(user.Id);
                if (user.LastMessage != null)
                    user.LastMessage.Content = user.LastMessage.IsHiddenByModeration
                        ? T(HiddenMessagePlaceholder)
                        : _encryptionService.Decrypt(user.LastMessage.Content);
            }

            return ServiceResult<List<ChatUserDto>>.Ok(result);
        }

        public async Task<ServiceResult<List<MessageDto>>> GetChatHistoryAsync(string currentUserId, string otherUserId)
        {
            var accessCheck = await ValidateChatAccessAsync(currentUserId);
            if (!accessCheck.Success)
                return ServiceResult<List<MessageDto>>.Fail(accessCheck.Message!, resultType: accessCheck.ResultType);

            _logger.LogInformation("Fetching chat history between {UserId} and {OtherUserId}", currentUserId, otherUserId);
            
            var messages = await _chatRepo.GetMessagesBetweenUsersAsync(currentUserId, otherUserId);

            var result = messages.Select(m => new MessageDto
            {
                Id = m.Id,
                SenderId = m.SenderId.ToString(),
                ReceiverId = m.ReceiverId.ToString(),
                Content = m.IsHiddenByModeration ? T(HiddenMessagePlaceholder) : _encryptionService.Decrypt(m.Content),
                SentAt = m.SentAt,
                IsRead = m.ReadAt.HasValue,
                IsHiddenByModeration = m.IsHiddenByModeration
            }).ToList();

            return ServiceResult<List<MessageDto>>.Ok(result);
        }
        #endregion

        
        #region Messages Page
        public async Task<ServiceResult<MessageDto>> SendMessageAsync(string senderId, string receiverId, string content)
        {
            _logger.LogInformation("Sending message from {SenderId} to {ReceiverId}", senderId, receiverId);

            var accessCheck = await ValidateChatAccessAsync(senderId);
            if (!accessCheck.Success)
                return ServiceResult<MessageDto>.Fail(accessCheck.Message!, resultType: accessCheck.ResultType);

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

            var senderUser = await _dbContext.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == senderGuid);
            if (senderUser == null)
            {
                _logger.LogWarning("Sender user {SenderId} not found", senderId);
                return ServiceResult<MessageDto>.Fail("Sender user not found", resultType: ServiceResultType.Unauthorized);
            }

            var receiverUser = await _dbContext.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == receiverGuid);
            if (receiverUser == null)
            {
                _logger.LogWarning("Receiver user {ReceiverId} not found", receiverId);
                return ServiceResult<MessageDto>.Fail("Receiver user not found", resultType: ServiceResultType.NotFound);
            }

            if (receiverUser.AccountStatus == Enums.Account.AccountStatus.Banned)
            {
                _logger.LogWarning("Cannot send message to banned user {ReceiverId}", receiverId);
                return ServiceResult<MessageDto>.Fail("Cannot send messages to a banned user.", resultType: ServiceResultType.Forbidden);
            }

            // Prevent sending messages to soft-deleted users
            if (receiverUser.DeletedAt != null)
            {
                _logger.LogWarning("Cannot send message to deleted user {ReceiverId}", receiverId);
                return ServiceResult<MessageDto>.Fail("Cannot send message to a deleted user");
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
            if (!_tracker.IsUserInChatWith(receiverId, senderId))
            {
                await _notificationService.SendNotificationAsync( new NotificationRequestDto
                {
                    UserId = receiverId,
                    UserType = NotificationUserType.General,
                    Type = NotificationType.NewMessage,

                    TitleKey = "NOTIFICATION_NEW_MESSAGE_TITLE",
                    BodyKey = "NOTIFICATION_NEW_MESSAGE_BODY",
                    LocalizationArguments = new() { $"{senderUser.FirstName} {senderUser.LastName}" },
                    Title = "New Message",
                    Body = $"You have a new message from {senderUser.FirstName} {senderUser.LastName}",
                    Data = new Dictionary<string, string>
                    {
                        { "SenderId", senderId },
                        { "SenderName", $"{senderUser.FirstName} {senderUser.LastName}" },
                        { "Content", content }
                    },

                    ActionType = NotificationActionType.ChatUser,
                    ActionId = senderId,
                });
            }


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
                IsRead = message.ReadAt.HasValue,
                IsHiddenByModeration = false
            };

            return ServiceResult<MessageDto>.Ok(dto, code: "ZZ_CHAT_MESSAGE_SENT_SUCCESSFULLY");
        }

        public async Task<ServiceResult<bool>> MarkChatAsReadAsync(string currentUserId, string senderId)
        {
            var accessCheck = await ValidateChatAccessAsync(currentUserId);
            if (!accessCheck.Success)
                return ServiceResult<bool>.Fail(accessCheck.Message!, resultType: accessCheck.ResultType);

            _logger.LogInformation("Marking messages from {SenderId} to {ReceiverId} as read", senderId, currentUserId);

            await _chatRepo.MarkMessagesAsReadAsync(senderId: senderId, receiverId: currentUserId);
            return ServiceResult<bool>.Ok(true, code: "ZZ_CHAT_MESSAGES_MARKED_AS_READ_SUCCESSFULLY");
        }
        #endregion

        private async Task<ServiceResult<bool>> ValidateChatAccessAsync(string userId)
        {
            if (!Guid.TryParse(userId, out var parsedUserId))
                return ServiceResult<bool>.Fail("Invalid userId format", resultType: ServiceResultType.BadRequest);

            var user = await _dbContext.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == parsedUserId);

            if (user == null || user.DeletedAt != null)
                return ServiceResult<bool>.Fail("User not found.", resultType: ServiceResultType.Unauthorized);

            if (user.AccountStatus == Enums.Account.AccountStatus.Banned)
                return ServiceResult<bool>.Fail("Banned accounts cannot use chat.", resultType: ServiceResultType.Forbidden);

            return ServiceResult<bool>.Ok(true);
        }

        private string T(string literal) => _localizer.LocalizeLiteral(literal);
    }
}
