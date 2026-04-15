using MARN_API.Enums.Notification;

namespace MARN_API.DTOs.Notification
{
    public class NotificationRequestDto
    {
        public string ReceiverId { get; set; } = null!;
        public string? SenderId { get; set; } = null!;
        public NotificationType Type { get; set; }

        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;

        public object? Data { get; set; } 
        public bool SaveInDB { get; set; } = true;
    }
}
