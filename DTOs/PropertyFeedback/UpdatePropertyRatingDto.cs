using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.PropertyFeedback
{
    public class UpdatePropertyRatingDto
    {
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
    }
}
