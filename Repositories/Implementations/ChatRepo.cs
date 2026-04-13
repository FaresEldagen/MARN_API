using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MARN_API.Data;
using MARN_API.Models;
using Microsoft.EntityFrameworkCore;

using MARN_API.Repositories.Interfaces;
using MARN_API.DTOs.Chat;

namespace MARN_API.Repositories.Implementations
{
    public class ChatRepo : IChatRepo
    {
        private readonly AppDbContext Context;
        public ChatRepo(AppDbContext context)
        {
            Context = context;
        }


        #region Chats Page
        public async Task<List<ChatUserDto>> GetActiveChatUsersWithUnreadCountAsync(string currentUserId)
        {
            var currentUserGuid = Guid.Parse(currentUserId);

            var userIdsWithChats = await Context.Messages
                .Where(m => m.SenderId == currentUserGuid || m.ReceiverId == currentUserGuid)
                .Select(m => m.SenderId == currentUserGuid ? m.ReceiverId : m.SenderId)
                .Distinct()
                .ToListAsync();

            var usersWithCounts = await Context.Users
                .Where(u => userIdsWithChats.Contains(u.Id))
                .Select(u => new ChatUserDto
                {
                    Id = u.Id.ToString(),
                    UserName = $"{u.FirstName} {u.LastName}",
                    ProfileImage = u.ProfileImage,

                    UnreadCount = Context.Messages.Count(m => m.SenderId == u.Id && m.ReceiverId == currentUserGuid && !m.IsRead),

                    LastMessage = Context.Messages
                        .Where(m =>
                            (m.SenderId == u.Id && m.ReceiverId == currentUserGuid) ||
                            (m.SenderId == currentUserGuid && m.ReceiverId == u.Id))
                        .OrderByDescending(m => m.SentAt)
                        .Select(m => new LastMessageDto
                        {
                            Content = m.Content,
                            SentAt = m.SentAt,
                            IsMine = m.SenderId == currentUserGuid
                        })
                        .FirstOrDefault()!,
                })
                .ToListAsync();

            return usersWithCounts;
        }

        public async Task<List<ChatUserDto>> SearchUsersAsync(string currentUserId, string query, int limit)
        {
            //var userIdsWithChats = await Context.Messages
            //    .Where(m => m.SenderId.ToString() == currentUserId || m.ReceiverId.ToString() == currentUserId)
            //    .Select(m => m.SenderId.ToString() == currentUserId ? m.ReceiverId : m.SenderId)
            //    .Distinct()
            //    .ToListAsync();
            var currentUserGuid = Guid.Parse(currentUserId);

            var usersWithCounts = await Context.Users
                .Where(u => 
                    u.Id != currentUserGuid &&
                    //userIdsWithChats.Contains(u.Id) &&
                    ($"{u.FirstName} {u.LastName}"!.Contains(query) || u.Email!.Contains(query)))
                .Select(u => new ChatUserDto
                {
                    Id = u.Id.ToString(),
                    UserName = $"{u.FirstName} {u.LastName}",
                    ProfileImage = u.ProfileImage,

                    UnreadCount = Context.Messages.Count(m => m.SenderId == u.Id && m.ReceiverId == currentUserGuid && !m.IsRead),

                    LastMessage = Context.Messages
                        .Where(m =>
                            (m.SenderId == u.Id && m.ReceiverId == currentUserGuid) ||
                            (m.SenderId == currentUserGuid && m.ReceiverId == u.Id))
                        .OrderByDescending(m => m.SentAt)
                        .Select(m => new LastMessageDto
                        {
                            Content = m.Content,
                            SentAt = m.SentAt,
                            IsMine = m.SenderId == currentUserGuid
                        })
                        .FirstOrDefault(),
                })
                .Take(limit)
                .ToListAsync();

            return usersWithCounts;
        }

        public async Task<List<Message>> GetMessagesBetweenUsersAsync(string userId1, string userId2)
        {
            var guid1 = Guid.Parse(userId1);
            var guid2 = Guid.Parse(userId2);

            return await Context.Messages
                .Where(m =>
                    (m.SenderId == guid1 && m.ReceiverId == guid2) ||
                    (m.SenderId == guid2 && m.ReceiverId == guid1))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }
        #endregion


        #region Messages Page
        public async Task<Message> AddMessageAsync(Message message)
        {
            Context.Messages.Add(message);
            await Context.SaveChangesAsync();
            return message;
        }

        public async Task MarkMessagesAsReadAsync(string senderId, string receiverId)
        {
            var senderGuid = Guid.Parse(senderId);
            var receiverGuid = Guid.Parse(receiverId);

            await Context.Messages
                .Where(m => m.SenderId == senderGuid && m.ReceiverId == receiverGuid && !m.IsRead)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(m => m.IsRead, true)
                    .SetProperty(m => m.ReadAt, DateTime.UtcNow));
        }
        #endregion
    }
}
