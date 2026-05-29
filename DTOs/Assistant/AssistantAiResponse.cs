namespace MARN_API.DTOs.Assistant
{
    public class AssistantAiResponse
    {
        public string Content { get; set; } = string.Empty;
        public List<string> ImagePaths { get; set; } = [];
    }
}
