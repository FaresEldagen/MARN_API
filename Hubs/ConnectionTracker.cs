using System.Collections.Concurrent;

namespace MARN_API.Hubs
{
    public class ConnectionTracker
    {
        #region Online Users
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

            // Clean up stale entry if user is fully offline
            if (count == 0)
                OnlineUsers.TryRemove(userId, out _);

            return count == 0;
        }
        
        public bool IsOnline(string userId)
        {
            return OnlineUsers.TryGetValue(userId, out var count) && count > 0;
        }
        #endregion


        #region Active Chats
        // Maps ReceiverId to a set of UserIds they are currently actively chatting with
        public ConcurrentDictionary<string, HashSet<string>> ActiveChattingUsers { get; } = new();

        public void SetActiveChat(string userId, string otherUserId)
        {
            var chats = ActiveChattingUsers.GetOrAdd(userId, _ => new HashSet<string>());

            lock (chats)
            {
                chats.Add(otherUserId);
            }
        }

        public void RemoveActiveChat(string userId, string otherUserId)
        {
            if (ActiveChattingUsers.TryGetValue(userId, out var chats))
            {
                lock (chats)
                {
                    chats.Remove(otherUserId);

                    if (chats.Count == 0)
                        ActiveChattingUsers.TryRemove(userId, out _);
                }
            }
        }

        public bool IsUserInChatWith(string userId, string otherUserId)
        {
            if (ActiveChattingUsers.TryGetValue(userId, out var chats))
            {
                lock (chats)
                {
                    return chats.Contains(otherUserId);
                }
            }

            return false;
        }
        #endregion
    }
}
