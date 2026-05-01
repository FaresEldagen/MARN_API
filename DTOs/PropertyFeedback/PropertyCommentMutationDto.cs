using System;

namespace MARN_API.DTOs.PropertyFeedback
{
    public class PropertyCommentMutationDto
    {
        public long CommentId { get; set; }
        public long PropertyId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
