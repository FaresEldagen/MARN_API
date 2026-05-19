using MARN_API.Enums.Notification;

namespace MARN_API.DTOs.Notification
{
    public class NotificationRequestDto
    {
        public string UserId { get; set; } = string.Empty;
        public NotificationUserType UserType { get; set; }
        public NotificationType Type { get; set; }

        public string? TitleKey { get; set; }
        public string? BodyKey { get; set; }
        public List<string>? LocalizationArguments { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public object? Data { get; set; } = null;

        public NotificationActionType? ActionType { get; set; } = null;
        public string? ActionId { get; set; } = null;

        public bool SaveInDB { get; set; } = true;
    }
}
