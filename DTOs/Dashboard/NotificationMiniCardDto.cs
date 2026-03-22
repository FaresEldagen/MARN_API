using MARN_API.Enums;

namespace MARN_API.DTOs.Dashboard
{
    public class NotificationMiniCardDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
