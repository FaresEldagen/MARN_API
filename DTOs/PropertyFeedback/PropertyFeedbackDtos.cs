using System;
using System.ComponentModel.DataAnnotations;
using MARN_API.DTOs.Common;

namespace MARN_API.DTOs.PropertyFeedback
{
    public class PropertyFeedbackRequestDto
    {
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [StringLength(1000, ErrorMessage = "Comment content must be 1000 characters or less.")]
        public string? Content { get; set; }
    }

    public class PropertyFeedbackDto
    {
        public long FeedbackId { get; set; }
        public long PropertyId { get; set; }
        public Guid UserId { get; set; }
        public string UserDisplayName { get; set; } = string.Empty;
        public string? UserProfileImage { get; set; }
        public int Rating { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class PropertyFeedbackSummaryDto
    {
        public float AverageRating { get; set; }
        public int RatingsCount { get; set; }
        public int CommentsCount { get; set; }
        public PropertyFeedbackDto? CurrentUserFeedback { get; set; }
        public PagedResult<PropertyFeedbackDto> Feedback { get; set; } = new();
    }
}
