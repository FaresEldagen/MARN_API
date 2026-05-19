using MARN_API.Enums.Notification;

namespace MARN_API.DTOs.Notification
{
    public class NotificationCardDto
    {
        public long Id { get; set; }
        public NotificationType Type { get; set; }
        public string TypeDisplayName { get; set; } = string.Empty;

        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public object? Data { get; set; }

        public NotificationActionType? ActionType { get; set; }
        public string ActionTypeDisplayName { get; set; } = string.Empty;
        public string? ActionId { get; set; }

        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
