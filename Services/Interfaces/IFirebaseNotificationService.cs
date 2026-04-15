namespace MARN_API.Services.Interfaces
{
    public interface IFirebaseNotificationService
    {
        Task<List<string>> SendNotificationAsync(List<string> deviceTokens, string title, string body);
    }
}
