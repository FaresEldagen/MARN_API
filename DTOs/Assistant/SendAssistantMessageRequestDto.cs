using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.Assistant
{
    public class SendAssistantMessageRequestDto
    {
        public Guid? SessionId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
