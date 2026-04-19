using MARN_API.Enums.Account;

namespace MARN_API.DTOs.Chat
{
    public class ChatUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? ProfileImage { get; set; }

        public bool IsOnline { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int UnreadCount { get; set; }
        public LastMessageDto? LastMessage { get; set; }
    }
}
