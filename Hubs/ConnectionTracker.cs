using System.Collections.Concurrent;

namespace MARN_API.Hubs
{
    public class ConnectionTracker
    {
        // Maps UserId to the number of active connections they have
        public ConcurrentDictionary<string, int> OnlineUsers { get; } = new();

        public bool UserConnected(string userId)
        {
            var count = OnlineUsers.AddOrUpdate(userId, 1, (_, currentCount) => currentCount + 1);
            // If the count becomes exactly 1, they just came online.
            return count == 1;
        }

        public bool UserDisconnected(string userId)
        {
            var count = OnlineUsers.AddOrUpdate(userId, 0, (_, currentCount) => 
            {
                return currentCount > 0 ? currentCount - 1 : 0;
            });

            // If the count becomes exactly 0 (or was already 0), they are offline.
            // Returning true ensures we definitely broadcast offline if they've disconnected fully.
            return count == 0;
        }
        
        public bool IsOnline(string userId)
        {
            return OnlineUsers.TryGetValue(userId, out var count) && count > 0;
        }
    }
}
