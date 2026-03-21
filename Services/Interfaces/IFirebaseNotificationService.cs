using System.Collections.Generic;
using System.Threading.Tasks;

namespace MARN_API.Services
{
    public interface IFirebaseNotificationService
    {
        Task SendNotificationAsync(List<string> deviceTokens, string title, string body);
    }
}
