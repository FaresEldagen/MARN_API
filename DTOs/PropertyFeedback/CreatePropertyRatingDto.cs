using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.PropertyFeedback
{
    public class CreatePropertyRatingDto
    {
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
    }
}
