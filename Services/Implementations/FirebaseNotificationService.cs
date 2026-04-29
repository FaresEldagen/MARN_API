using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace MARN_API.Services.Implementations
{
    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        private readonly ILogger<FirebaseNotificationService> _logger;
        private readonly bool _isFirebaseInitialized;

        public FirebaseNotificationService(ILogger<FirebaseNotificationService> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            try
            {
                if (FirebaseApp.DefaultInstance == null)
                {
                    string path = Path.Combine(env.ContentRootPath, "serviceAccountKey.json");
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = CredentialFactory.FromFile<ServiceAccountCredential>(path).ToGoogleCredential(),
                    });
                }
                _isFirebaseInitialized = true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("FCM Not Initialized. Push Notifications are disabled. Error: " + ex.Message);
                _isFirebaseInitialized = false;
            }
        }

        public async Task<List<string>> SendNotificationAsync(List<string> deviceTokens, string title, string body)
        {
            var invalidTokens = new List<string>();

            if (!_isFirebaseInitialized || deviceTokens == null || deviceTokens.Count == 0) 
                return invalidTokens;

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

                for (int i = 0; i < response.Responses.Count; i++)
                {
                    var resp = response.Responses[i];

                    if (!resp.IsSuccess)
                    {
                        var errorCode = resp.Exception?.MessagingErrorCode;

                        if (errorCode == MessagingErrorCode.Unregistered ||
                            errorCode == MessagingErrorCode.InvalidArgument)
                        {
                            invalidTokens.Add(deviceTokens[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send FCM Push Notification: " + ex.Message);
            }

            return invalidTokens;
        }
    }
}
