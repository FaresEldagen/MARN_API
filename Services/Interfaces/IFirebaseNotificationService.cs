namespace MARN_API.Services.Interfaces
{
    public interface IFirebaseNotificationService
    {
        Task SendNotificationAsync(List<string> deviceTokens, string title, string body);
    }
}
