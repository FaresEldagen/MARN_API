using System.Globalization;
using MARN_API.DTOs.Notification;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface INotificationContentLocalizer
    {
        RenderedNotificationContent Render(NotificationRequestDto request, CultureInfo? culture = null);
        RenderedNotificationContent Render(Notification notification, CultureInfo? culture = null);
    }

    public sealed class RenderedNotificationContent
    {
        public string Title { get; init; } = string.Empty;
        public string Body { get; init; } = string.Empty;
    }
}
