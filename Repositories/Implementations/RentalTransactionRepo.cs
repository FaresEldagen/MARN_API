using Microsoft.EntityFrameworkCore;
using MARN_API.Data;
using MARN_API.DTOs.Common;
using MARN_API.Enums;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;

namespace MARN_API.Repositories.Implementations
{
    public class RentalTransactionRepo : IRentalTransactionRepo
    {
        private readonly AppDbContext _context;

        public RentalTransactionRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RentalTransaction record)
        {
            _context.RentalTransactions.Add(record);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RentalTransaction record)
        {
            _context.RentalTransactions.Update(record);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<RentalTransaction>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.RentalTransactions
                .Include(r => r.Property)
                .OrderByDescending(r => r.CreatedAt);

            return await CreatePagedResultAsync(query, pageNumber, pageSize);
        }

        public async Task<PagedResult<RentalTransaction>> GetByUserIdAsync(Guid userId, int pageNumber, int pageSize)
        {
            var query = _context.RentalTransactions
                .Include(r => r.Property)
                .Where(r => r.OwnerId == userId || r.RenterId == userId)
                .OrderByDescending(r => r.CreatedAt);

            return await CreatePagedResultAsync(query, pageNumber, pageSize);
        }

        public async Task<PagedResult<RentalTransaction>> GetByPropertyIdAsync(long propertyId, int pageNumber, int pageSize)
        {
            var query = _context.RentalTransactions
                .Include(r => r.Property)
                .Where(r => r.PropertyId == propertyId)
                .OrderByDescending(r => r.CreatedAt);

            return await CreatePagedResultAsync(query, pageNumber, pageSize);
        }

        public Task<RentalTransaction?> GetByIdAsync(Guid id)
        {
            return _context.RentalTransactions
                .Include(r => r.Property)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task<RentalTransaction?> GetByPaymentRecordIdAsync(long paymentRecordId)
        {
            return _context.RentalTransactions
                .FirstOrDefaultAsync(r => r.PaymentId == paymentRecordId);
        }

        public Task<RentalTransaction?> GetBySessionIdAsync(string sessionId)
        {
            return _context.RentalTransactions
                .FirstOrDefaultAsync(r => r.StripeSessionId == sessionId);
        }

        public Task<RentalTransaction?> GetByContractIdAsync(long contractId)
        {
            return _context.RentalTransactions
                .FirstOrDefaultAsync(r => r.ContractId == contractId);
        }

        public Task<RentalTransaction?> GetActiveInitiatedSessionAsync(Guid renterId, long propertyId)
        {
            var cutoff = DateTime.UtcNow.AddHours(-24);

            return _context.RentalTransactions
                .Where(r => r.RenterId == renterId && r.PropertyId == propertyId)
                .Where(r => r.Status == RentalTransactionStatus.Initiated && r.CreatedAt > cutoff)
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefaultAsync();
        }

        private static async Task<PagedResult<RentalTransaction>> CreatePagedResultAsync(IQueryable<RentalTransaction> query, int pageNumber, int pageSize)
        {
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<RentalTransaction>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }
    }
}
