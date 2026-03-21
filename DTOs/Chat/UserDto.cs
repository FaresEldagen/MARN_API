namespace MARN_API.DTOs.Chat
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool IsOnline { get; set; }
        public int UnreadCount { get; set; }
    }
}
