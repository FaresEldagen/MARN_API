using Microsoft.EntityFrameworkCore;
using MARN_API.Data;
using MARN_API.DTOs.Common;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;

namespace MARN_API.Repositories.Implementations
{
    public class WorkflowPaymentRepo : IWorkflowPaymentRepo
    {
        private readonly AppDbContext _context;

        public WorkflowPaymentRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> CreatePaymentRecordAsync(Payment record)
        {
            _context.Payments.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public Task<Payment?> GetPaymentByIdAsync(long paymentId)
        {
            return BuildBaseQuery()
                .FirstOrDefaultAsync(p => p.Id == paymentId);
        }

        public Task<Payment?> GetPaymentBySessionIdAsync(string sessionId)
        {
            return BuildBaseQuery()
                .FirstOrDefaultAsync(p => p.StripeSessionId == sessionId);
        }

        public async Task<PagedResult<Payment>> GetPaymentsByUserIdAsync(Guid userId, int pageNumber, int pageSize)
        {
            var query = BuildBaseQuery()
                .Where(p => p.OwnerId == userId || p.RenterId == userId)
                .OrderByDescending(p => p.CreatedAt);

            return await CreatePagedResultAsync(query, pageNumber, pageSize);
        }

        public async Task<PagedResult<Payment>> GetPaymentsByPropertyIdAsync(long propertyId, int pageNumber, int pageSize)
        {
            var query = BuildBaseQuery()
                .Where(p => p.PropertyId == propertyId)
                .OrderByDescending(p => p.CreatedAt);

            return await CreatePagedResultAsync(query, pageNumber, pageSize);
        }

        public async Task<PagedResult<Payment>> GetAllPaymentsAsync(int pageNumber, int pageSize)
        {
            var query = BuildBaseQuery()
                .OrderByDescending(p => p.CreatedAt);

            return await CreatePagedResultAsync(query, pageNumber, pageSize);
        }

        public async Task UpdatePaymentRecordAsync(Payment record)
        {
            _context.Payments.Update(record);
            await _context.SaveChangesAsync();
        }

        private IQueryable<Payment> BuildBaseQuery()
        {
            return _context.Payments
                .Include(p => p.Contract)
                .Include(p => p.Property)
                .Include(p => p.Renter);
        }

        private static async Task<PagedResult<Payment>> CreatePagedResultAsync(IQueryable<Payment> query, int pageNumber, int pageSize)
        {
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<Payment>
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
