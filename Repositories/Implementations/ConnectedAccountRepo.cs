using Microsoft.EntityFrameworkCore;
using MARN_API.Data;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;

namespace MARN_API.Repositories.Implementations
{
    public class ConnectedAccountRepo : IConnectedAccountRepo
    {
        private readonly AppDbContext _context;

        public ConnectedAccountRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ConnectedAccount record)
        {
            _context.ConnectedAccounts.Add(record);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ConnectedAccount record)
        {
            _context.ConnectedAccounts.Update(record);
            await _context.SaveChangesAsync();
        }

        public Task<ConnectedAccount?> GetByIdAsync(Guid id)
        {
            return _context.ConnectedAccounts
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<ConnectedAccount?> GetByApplicationUserIdAsync(Guid applicationUserId)
        {
            return _context.ConnectedAccounts
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == applicationUserId);
        }

        public async Task<IEnumerable<ConnectedAccount>> GetAllAsync()
        {
            return await _context.ConnectedAccounts
                .Include(c => c.ApplicationUser)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
