using AutoMapper;
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
        private readonly IFirebaseNotificationService _fcmService;
        private readonly ConnectionTracker _tracker;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            INotificationRepo notificationRepo,
            IFirebaseNotificationService fcmService,
            ConnectionTracker tracker,
            IHubContext<NotificationHub> notificationHub,
            IMapper mapper,
            ILogger<NotificationService> logger)
        {
            _notificationRepo = notificationRepo;
            _tracker = tracker;
            _fcmService = fcmService;
            _notificationHub = notificationHub;
            _mapper = mapper;
            _logger = logger;
        }


        #region Notification
        public async Task SendNotificationAsync(NotificationRequestDto request)
        {
            // Save in DB
            if (request.SaveInDB)
            {
                var notification = _mapper.Map<Notification>(request);
                await _notificationRepo.AddAsync(notification);
            }


            if (_tracker.IsOnline(request.UserId))
            {
                // Send real-time notification via SignalR
                await _notificationHub.Clients.User(request.UserId)
                    .SendAsync("ReceiveNotification", request);
            }
            else
            {
                // Send FCM
                var tokens = await _notificationRepo.GetUserDeviceTokensAsync(request.UserId);

                if (tokens.Any())
                {
                    _logger.LogInformation("Receiver {ReceiverId} is offline, sending FCM notification", request.UserId);

                    var invalidTokens = await _fcmService.SendNotificationAsync(tokens, request.Title, request.Body);

                    foreach (var invalidToken in invalidTokens)
                    {
                        _logger.LogWarning("Removing invalid FCM token for user {ReceiverId}: {Token}", request.UserId, invalidToken);
                        await _notificationRepo.RemoveUserDeviceAsync(request.UserId, invalidToken);
                    }
                }
            }
        }


        public async Task<ServiceResult<List<NotificationCardDto>>> GetUserNotificationsAsync(Guid userId)
        {
            _logger.LogInformation("Fetching notifications for user {UserId}", userId);

            var notifications = await _notificationRepo.GetAllNotificationsAsync(userId.ToString());
            var result = _mapper.Map<List<NotificationCardDto>>(notifications);

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
