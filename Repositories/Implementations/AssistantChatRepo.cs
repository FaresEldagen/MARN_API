using MARN_API.Data;
using MARN_API.DTOs.Assistant;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class AssistantChatRepo : IAssistantChatRepo
    {
        private readonly AppDbContext _context;

        public AssistantChatRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AssistantSession?> GetSessionForUserAsync(Guid sessionId, Guid userId)
        {
            return await _context.AssistantSessions
                .FirstOrDefaultAsync(s => s.SessionId == sessionId && s.UserId == userId);
        }

        public async Task<List<AssistantSessionDto>> GetSessionsForUserAsync(Guid userId)
        {
            return await _context.AssistantSessions
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.UpdatedAt)
                .Select(s => new AssistantSessionDto
                {
                    SessionId = s.SessionId,
                    SessionName = s.SessionName,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    LastMessagePreview = _context.AssistantMessages
                        .Where(m => m.SessionId == s.SessionId && !m.ToolOnly)
                        .OrderByDescending(m => m.CreatedAt)
                        .Select(m => m.Content)
                        .FirstOrDefault(),
                    LastMessageAt = _context.AssistantMessages
                        .Where(m => m.SessionId == s.SessionId && !m.ToolOnly)
                        .OrderByDescending(m => m.CreatedAt)
                        .Select(m => (DateTime?)m.CreatedAt)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }

        public async Task<List<AssistantMessage>> GetVisibleMessagesAsync(Guid sessionId, Guid userId)
        {
            return await _context.AssistantMessages
                .Where(m => m.SessionId == sessionId && m.UserId == userId && !m.ToolOnly)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<AssistantSession> AddSessionAsync(AssistantSession session)
        {
            _context.AssistantSessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<AssistantMessage> AddMessageAsync(AssistantMessage message)
        {
            _context.AssistantMessages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task UpdateSessionAsync(AssistantSession session)
        {
            _context.AssistantSessions.Update(session);
            await _context.SaveChangesAsync();
        }
    }
}
