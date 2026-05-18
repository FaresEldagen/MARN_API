using System.Globalization;
using System.Text.Json;
using MARN_API.DTOs.Notification;
using MARN_API.Models;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class NotificationContentLocalizer : INotificationContentLocalizer
    {
        private readonly IAppTextLocalizer _localizer;

        public NotificationContentLocalizer(IAppTextLocalizer localizer)
        {
            _localizer = localizer;
        }

        public RenderedNotificationContent Render(NotificationRequestDto request, CultureInfo? culture = null)
        {
            var arguments = request.LocalizationArguments?.Cast<object?>().ToArray() ?? Array.Empty<object?>();

            return new RenderedNotificationContent
            {
                Title = ResolveText(request.TitleKey, request.Title, culture, arguments),
                Body = ResolveText(request.BodyKey, request.Body, culture, arguments)
            };
        }

        public RenderedNotificationContent Render(Notification notification, CultureInfo? culture = null)
        {
            var arguments = DeserializeArguments(notification.LocalizationArgumentsJson);

            return new RenderedNotificationContent
            {
                Title = ResolveText(notification.TitleKey, notification.Title, culture, arguments),
                Body = ResolveText(notification.BodyKey, notification.Body, culture, arguments)
            };
        }

        private string ResolveText(string? key, string? fallback, CultureInfo? culture, object?[] arguments)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                return _localizer.GetOrFallback(key, fallback ?? string.Empty, culture, arguments);
            }

            return _localizer.LocalizeMessage(null, fallback, culture, arguments);
        }

        private static object?[] DeserializeArguments(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return Array.Empty<object?>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<string>>(json)?.Cast<object?>().ToArray() ?? Array.Empty<object?>();
            }
            catch
            {
                return Array.Empty<object?>();
            }
        }
    }
}
