using System;

namespace MARN_API.DTOs.PropertyFeedback
{
    public class PropertyCommentDto
    {
        public long CommentId { get; set; }
        public Guid UserId { get; set; }
        public string UserDisplayName { get; set; } = string.Empty;
        public string? UserProfileImage { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
