namespace MARN_API.Models
{
    public static class AssistantMessageRoles
    {
        public const string User = "user";
        public const string Assistant = "assistant";
        public const string Tool = "tool";

        public static bool IsValid(string role)
        {
            return role == User || role == Assistant || role == Tool;
        }
    }
}
