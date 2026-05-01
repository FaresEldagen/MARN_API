using System;

namespace MARN_API.DTOs.PropertyFeedback
{
    public class PropertyRatingDto
    {
        public long RatingId { get; set; }
        public long PropertyId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
