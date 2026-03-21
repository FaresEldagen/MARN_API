using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MARN_API.Data;
using MARN_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _context;

        public ChatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Message> AddMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<Message>> GetMessagesBetweenUsersAsync(string userId1, string userId2)
        {
            return await _context.Messages
                .Where(m =>
                    (m.SenderId.ToString() == userId1 && m.ReceiverId.ToString() == userId2) ||
                    (m.SenderId.ToString() == userId2 && m.ReceiverId.ToString() == userId1))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task MarkMessagesAsReadAsync(string senderId, string receiverId)
        {
            var unreadMessages = await _context.Messages
                .Where(m => m.SenderId.ToString() == senderId && m.ReceiverId.ToString() == receiverId && !m.IsRead)
                .ToListAsync();

            if (unreadMessages.Any())
            {
                foreach (var msg in unreadMessages)
                {
                    msg.IsRead = true;
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<(ApplicationUser User, int UnreadCount)>> GetActiveChatUsersWithUnreadCountAsync(string currentUserId)
        {
            var userIdsWithChats = await _context.Messages
                .Where(m => m.SenderId.ToString() == currentUserId || m.ReceiverId.ToString() == currentUserId)
                .Select(m => m.SenderId.ToString() == currentUserId ? m.ReceiverId : m.SenderId)
                .Distinct()
                .ToListAsync();

            var usersWithCounts = await _context.Users
                .Where(u => userIdsWithChats.Contains(u.Id))
                .Select(u => new
                {
                    User = u,
                    UnreadCount = _context.Messages.Count(m => m.SenderId == u.Id && m.ReceiverId.ToString() == currentUserId && !m.IsRead)
                })
                .ToListAsync();

            return usersWithCounts.Select(x => (x.User, x.UnreadCount)).ToList();
        }

        public async Task<List<ApplicationUser>> SearchUsersAsync(string currentUserId, string query, int limit)
        {
            return await _context.Users
                .Where(u => u.Id.ToString() != currentUserId && (u.UserName!.Contains(query) || u.Email!.Contains(query)))
                .Take(limit)
                .ToListAsync();
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task AddOrUpdateUserDeviceAsync(string userId, string fcmToken)
        {
            var device = await _context.UserDevices.FirstOrDefaultAsync(d => d.FcmToken == fcmToken);
            if (device == null)
            {
                _context.UserDevices.Add(new UserDevice
                {
                    Id = System.Guid.NewGuid(),
                    UserId = userId,
                    FcmToken = fcmToken,
                    LastUpdated = System.DateTime.UtcNow
                });
            }
            else
            {
                // In case a device changes hands (logout and new user logs in on same phone)
                device.UserId = userId; 
                device.LastUpdated = System.DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetUserDeviceTokensAsync(string userId)
        {
            return await _context.UserDevices
                .Where(d => d.UserId == userId)
                .Select(d => d.FcmToken)
                .ToListAsync();
        }
    }
}
