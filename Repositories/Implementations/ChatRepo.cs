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
        private readonly IConfiguration _configuration;
        public ChatRepo(AppDbContext context, IConfiguration configuration)
        {
            Context = context;
            _configuration = configuration;
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

            var BaseUrl = _configuration["AppSettings:BaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured.");

            // Use IgnoreQueryFilters to include soft-deleted users who have chat history
            var usersWithCounts = await Context.Users
                .IgnoreQueryFilters()
                .Where(u => userIdsWithChats.Contains(u.Id))
                .Select(u => new ChatUserDto
                {
                    Id = u.Id.ToString(),
                    UserName = $"{u.FirstName} {u.LastName}",
                    ProfileImage = u.ProfileImage == null
                        ? null
                        : $"{BaseUrl}{u.ProfileImage}",

                    IsDeleted = u.DeletedAt != null,

                    UnreadCount = Context.Messages.Count(m => m.SenderId == u.Id && m.ReceiverId == currentUserGuid && !m.ReadAt.HasValue),

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
                .OrderByDescending(u => u.LastMessage!.SentAt)
                .ToListAsync();

            return usersWithCounts;
        }

        public async Task<List<ChatUserDto>> SearchUsersAsync(string currentUserId, string query, int limit)
        {
            var currentUserGuid = Guid.Parse(currentUserId);

            var BaseUrl = _configuration["AppSettings:BaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured.");

            // Get IDs of users who have exchanged messages with the current user
            var userIdsWithChats = await Context.Messages
                .Where(m => m.SenderId == currentUserGuid || m.ReceiverId == currentUserGuid)
                .Select(m => m.SenderId == currentUserGuid ? m.ReceiverId : m.SenderId)
                .Distinct()
                .ToListAsync();

            // Search non-deleted users (Global Query Filter applies automatically)
            var activeUsers = await Context.Users
                .Where(u =>
                    u.Id != currentUserGuid &&
                    (
                        u.FirstName.Contains(query) ||
                        u.LastName.Contains(query) ||
                        u.Email!.Contains(query)
                    ))
                .Select(u => new ChatUserDto
                {
                    Id = u.Id.ToString(),
                    UserName = $"{u.FirstName} {u.LastName}",
                    ProfileImage = u.ProfileImage == null
                        ? null
                        : $"{BaseUrl}{u.ProfileImage}",

                    IsDeleted = false,

                    UnreadCount = Context.Messages.Count(m => m.SenderId == u.Id && m.ReceiverId == currentUserGuid && !m.ReadAt.HasValue),

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
                .ToListAsync();

            // Also include deleted users who have chat history with the current user and match the query
            var deletedUsersWithChat = await Context.Users
                .IgnoreQueryFilters()
                .Where(u =>
                    u.DeletedAt != null &&
                    u.Id != currentUserGuid &&
                    userIdsWithChats.Contains(u.Id) &&
                    (
                        u.FirstName.Contains(query) ||
                        u.LastName.Contains(query) ||
                        u.Email!.Contains(query)
                    ))
                .Select(u => new ChatUserDto
                {
                    Id = u.Id.ToString(),
                    UserName = $"{u.FirstName} {u.LastName}",
                    ProfileImage = u.ProfileImage == null
                        ? null
                        : $"{BaseUrl}{u.ProfileImage}",

                    IsDeleted = true,

                    UnreadCount = Context.Messages.Count(m => m.SenderId == u.Id && m.ReceiverId == currentUserGuid && !m.ReadAt.HasValue),

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
                .ToListAsync();

            // Merge results: active users first, then deleted with chat history
            var combined = activeUsers.Concat(deletedUsersWithChat)
                .OrderByDescending(u => u.LastMessage == null ? DateTime.MinValue : u.LastMessage.SentAt)
                .Take(limit)
                .ToList();
            return combined;
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
                .Where(m => m.SenderId == senderGuid && m.ReceiverId == receiverGuid && !m.ReadAt.HasValue)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(m => m.ReadAt, DateTime.UtcNow));
        }
        #endregion
    }
}
