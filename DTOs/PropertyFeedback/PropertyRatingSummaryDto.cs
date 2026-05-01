namespace MARN_API.DTOs.PropertyFeedback
{
    public class PropertyRatingSummaryDto
    {
        public float AverageRating { get; set; }
        public int RatingsCount { get; set; }
        public int? CurrentUserRating { get; set; }
    }
}
