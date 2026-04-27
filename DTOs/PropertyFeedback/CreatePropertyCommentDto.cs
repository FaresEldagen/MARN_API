using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.PropertyFeedback
{
    public class CreatePropertyCommentDto
    {
        [Required(ErrorMessage = "Comment content is required.")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Comment content must be between 1 and 1000 characters.")]
        public string Content { get; set; } = string.Empty;
    }
}
