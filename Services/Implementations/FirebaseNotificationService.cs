using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        private readonly ILogger<FirebaseNotificationService> _logger;
        private readonly bool _isFirebaseInitialized;

        public FirebaseNotificationService(ILogger<FirebaseNotificationService> logger)
        {
            _logger = logger;
            try
            {
                // This expects the GOOGLE_APPLICATION_CREDENTIALS environment variable.
                // We gracefully fallback if the user hasn't downloaded their ServiceAccount.json yet.
                if (FirebaseApp.DefaultInstance == null)
                {
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.GetApplicationDefault(),
                    });
                }
                _isFirebaseInitialized = true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("FCM Not Initialized. Push Notifications are disabled until GOOGLE_APPLICATION_CREDENTIALS is set. Error: " + ex.Message);
                _isFirebaseInitialized = false;
            }
        }

        public async Task SendNotificationAsync(List<string> deviceTokens, string title, string body)
        {
            if (!_isFirebaseInitialized || deviceTokens == null || deviceTokens.Count == 0) return;

            var message = new MulticastMessage()
            {
                Tokens = deviceTokens,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };

            try
            {
                var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
                _logger.LogInformation($"Successfully dispatched FCM Push Notifications to {response.SuccessCount} devices.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send FCM Push Notification: " + ex.Message);
            }
        }
    }
}
