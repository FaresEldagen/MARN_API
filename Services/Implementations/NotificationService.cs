using MARN_API.DTOs.Notification;
using MARN_API.Enums.Notification;
using MARN_API.Hubs;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace MARN_API.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepo _notificationRepo;
        private readonly ConnectionTracker _tracker;
        private readonly IEncryptionService _encryptionService;
        private readonly IFirebaseNotificationService _fcmService;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            ConnectionTracker tracker,
            INotificationRepo notificationRepo,
            IEncryptionService encryptionService,
            IFirebaseNotificationService fcmService,
            IHubContext<NotificationHub> notificationHub,
            ILogger<NotificationService> logger)
        {
            _notificationRepo = notificationRepo;
            _tracker = tracker;
            _encryptionService = encryptionService;
            _fcmService = fcmService;
            _notificationHub = notificationHub;
            _logger = logger;
        }


        #region Notification
        public async Task SendNotificationAsync(NotificationRequestDto request)
        {
            // Special Conditions
            if (request.Type == NotificationType.NewMessage && 
                _tracker.IsUserInChatWith(request.ReceiverId, request.SenderId!))
            { return; }


            // Save in DB
            if (request.SaveInDB)
            {
                await _notificationRepo.AddAsync(new Notification
                {
                    UserId = Guid.Parse(request.ReceiverId),
                    Type = request.Type,
                    Title = request.Title,
                    Body = request.Body,
                    Data = request.Data != null ? JsonSerializer.Serialize(request.Data) : null
                });
            }


            if (_tracker.IsOnline(request.ReceiverId))
            {
                // Send real-time notification via SignalR
                await _notificationHub.Clients.User(request.ReceiverId)
                    .SendAsync("ReceiveNotification", request);
            }
            else
            {
                // Send FCM
                var tokens = await _notificationRepo.GetUserDeviceTokensAsync(request.ReceiverId);

                if (tokens.Any())
                {
                    _logger.LogInformation("Receiver {ReceiverId} is offline, sending FCM notification", request.ReceiverId);

                    var invalidTokens = await _fcmService.SendNotificationAsync(tokens, request.Title, request.Body);

                    foreach (var invalidToken in invalidTokens)
                    {
                        _logger.LogWarning("Removing invalid FCM token for user {ReceiverId}: {Token}", request.ReceiverId, invalidToken);
                        await _notificationRepo.RemoveUserDeviceAsync(request.ReceiverId, invalidToken);
                    }
                }
            }
        }


        public async Task<ServiceResult<List<NotificationCardDto>>> GetUserNotificationsAsync(Guid userId)
        {
            _logger.LogInformation("Fetching notifications for user {UserId}", userId);
            var notifications = await _notificationRepo.GetAllNotificationsAsync(userId.ToString());
            var result = notifications.Select(n => new NotificationCardDto
            {
                Id = n.Id,
                Type = n.Type,
                Title = n.Title,
                Body = n.Body,
                Data = n.Data != null ? JsonSerializer.Deserialize<Dictionary<string, string>>(n.Data) : null,
                IsRead = n.ReadAt.HasValue,
                CreatedAt = n.CreatedAt
            }).ToList();
            return ServiceResult<List<NotificationCardDto>>.Ok(result);
        }


        public async Task MarkAllNotificationsAsReadAsync(string currentUserId)
        {
            _logger.LogInformation("Marking notifications as read for user {UserId}", currentUserId);
            await _notificationRepo.MarkAllAsReadAsync(currentUserId);
        }

        public async Task MarkNotificationAsReadAsync(string currentUserId, long notificationId)
        {
            _logger.LogInformation("Marking notification {NotificationId} as read for user {UserId}", notificationId, currentUserId);
            await _notificationRepo.MarkAsReadAsync(currentUserId, notificationId);
        }
        #endregion


        #region FCM Device Tokens
        public async Task<ServiceResult<bool>> SaveDeviceTokenAsync(string userId, string fcmToken)
        {
            _logger.LogInformation("Saving FCM token for user {UserId}", userId);

            await _notificationRepo.AddOrUpdateUserDeviceAsync(userId, fcmToken);
            return ServiceResult<bool>.Ok(true);
        }

        public async Task<ServiceResult<bool>> RemoveDeviceTokenAsync(string userId, string fcmToken)
        {
            _logger.LogInformation("Removing FCM token for user {UserId}", userId);

            await _notificationRepo.RemoveUserDeviceAsync(userId, fcmToken);
            return ServiceResult<bool>.Ok(true);
        }
        #endregion
    }
}
