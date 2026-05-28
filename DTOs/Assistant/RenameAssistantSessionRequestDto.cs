using System.ComponentModel.DataAnnotations;

namespace MARN_API.DTOs.Assistant
{
    public class RenameAssistantSessionRequestDto
    {
        [Required]
        [MaxLength(200)]
        public string SessionName { get; set; } = string.Empty;
    }
}
