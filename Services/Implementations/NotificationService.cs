using MARN_API.Hubs;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepo _notificationRepo;
        private readonly ConnectionTracker _tracker;
        private readonly IEncryptionService _encryptionService;
        private readonly IFirebaseNotificationService _fcmService;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            ConnectionTracker tracker,
            INotificationRepo notificationRepo,
            IEncryptionService encryptionService,
            IFirebaseNotificationService fcmService,
            ILogger<NotificationService> logger)
        {
            _notificationRepo = notificationRepo;
            _tracker = tracker;
            _encryptionService = encryptionService;
            _fcmService = fcmService;
            _logger = logger;
        }


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

        public async Task SendMessageNotificationAsync(string receiverId, string senderName, string content)
        {
            if (!_tracker.IsOnline(receiverId))
            {
                var receiverTokens = await _notificationRepo.GetUserDeviceTokensAsync(receiverId);

                if (receiverTokens != null && receiverTokens.Any())
                {
                    _logger.LogInformation("Receiver {ReceiverId} is offline, sending FCM notification", receiverId);

                    await _fcmService.SendNotificationAsync(
                        receiverTokens,
                        $"New Message from {senderName}",
                        content
                    );
                }
            }
        }
    }
}
