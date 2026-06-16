using System.Collections.Concurrent;

namespace MARN_API.Hubs
{
    public class ConnectionTracker
    {
        #region Online Users
        // Maps UserId to the number of active connections they have
        public ConcurrentDictionary<string, int> OnlineUsers { get; } = new(StringComparer.OrdinalIgnoreCase);

        public bool UserConnected(string userId)
        {
            var count = OnlineUsers.AddOrUpdate(userId, 1, (_, currentCount) => currentCount + 1);

            // If the count becomes exactly 1, they just came online.
            return count == 1;
        }

        public bool UserDisconnected(string userId)
        {
            // Use an atomic loop to avoid the race condition where AddOrUpdate
            // could re-insert a removed key with value 0, then a concurrent
            // UserConnected gets its entry deleted by TryRemove.
            while (true)
            {
                if (!OnlineUsers.TryGetValue(userId, out var currentCount))
                    return false; // Already gone — no-op, don't fire offline again

                if (currentCount <= 1)
                {
                    // Try to atomically remove the entry
                    if (OnlineUsers.TryRemove(userId, out _))
                        return true; // Now fully offline
                    // Another thread changed it — retry
                }
                else
                {
                    var newCount = currentCount - 1;
                    if (OnlineUsers.TryUpdate(userId, newCount, currentCount))
                        return false; // Still online with other connections
                    // Another thread changed it — retry
                }
            }
        }
        
        public bool IsOnline(string userId)
        {
            return OnlineUsers.TryGetValue(userId, out var count) && count > 0;
        }
        #endregion


        #region Active Chats
        // Maps UserId -> (OtherUserId -> connection count viewing that chat)
        // Reference-counted so multiple devices can view the same chat simultaneously
        private ConcurrentDictionary<string, ConcurrentDictionary<string, int>> ActiveChattingUsers { get; } = new(StringComparer.OrdinalIgnoreCase);

        // Maps ConnectionId -> set of OtherUserIds that connection is viewing
        // Used to clean up only that connection's chats on disconnect
        private ConcurrentDictionary<string, HashSet<string>> ConnectionActiveChats { get; } = new(StringComparer.OrdinalIgnoreCase);

        public void SetActiveChat(string connectionId, string userId, string otherUserId)
        {
            // 1. Track which chats this specific connection is viewing
            var connChats = ConnectionActiveChats.GetOrAdd(connectionId, _ => new HashSet<string>(StringComparer.OrdinalIgnoreCase));
            lock (connChats)
            {
                if (!connChats.Add(otherUserId))
                    return; // This connection was already tracking this chat — no-op
            }

            // 2. Increment the reference count for (userId, otherUserId)
            var userChats = ActiveChattingUsers.GetOrAdd(userId, _ => new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase));
            userChats.AddOrUpdate(otherUserId, 1, (_, count) => count + 1);
        }

        public void RemoveActiveChat(string connectionId, string userId, string otherUserId)
        {
            // 1. Remove from this connection's tracking
            if (ConnectionActiveChats.TryGetValue(connectionId, out var connChats))
            {
                bool wasTracked;
                lock (connChats)
                {
                    wasTracked = connChats.Remove(otherUserId);
                    if (connChats.Count == 0)
                        ConnectionActiveChats.TryRemove(connectionId, out _);
                }

                if (!wasTracked)
                    return; // This connection wasn't tracking this chat — no-op
            }
            else
            {
                return;
            }

            // 2. Decrement the reference count for (userId, otherUserId)
            DecrementActiveChat(userId, otherUserId);
        }

        public void RemoveAllActiveChatsForConnection(string connectionId, string userId)
        {
            // Get and remove all chats this specific connection was viewing
            if (!ConnectionActiveChats.TryRemove(connectionId, out var connChats))
                return;

            HashSet<string> chatsCopy;
            lock (connChats)
            {
                chatsCopy = new HashSet<string>(connChats, StringComparer.OrdinalIgnoreCase);
            }

            // Decrement the reference count for each chat this connection had open
            foreach (var otherUserId in chatsCopy)
            {
                DecrementActiveChat(userId, otherUserId);
            }
        }

        public bool IsUserInChatWith(string userId, string otherUserId)
        {
            return ActiveChattingUsers.TryGetValue(userId, out var userChats)
                && userChats.TryGetValue(otherUserId, out var count)
                && count > 0;
        }

        private void DecrementActiveChat(string userId, string otherUserId)
        {
            if (!ActiveChattingUsers.TryGetValue(userId, out var userChats))
                return;

            var newCount = userChats.AddOrUpdate(otherUserId, 0, (_, count) => count > 0 ? count - 1 : 0);

            if (newCount == 0)
            {
                userChats.TryRemove(otherUserId, out _);

                if (userChats.IsEmpty)
                    ActiveChattingUsers.TryRemove(userId, out _);
            }
        }
        #endregion
    }
}
