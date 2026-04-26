using Microsoft.EntityFrameworkCore;
using MARN_API.Data;
using MARN_API.DTOs.Common;
using MARN_API.Enums;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;

namespace MARN_API.Repositories.Implementations
{
    public class WorkflowContractRepo : IWorkflowContractRepo
    {
        private readonly AppDbContext _context;

        public WorkflowContractRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Contract>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = BuildBaseQuery()
                .OrderByDescending(c => c.SubmittedAt);

            return await CreatePagedResultAsync(query, pageNumber, pageSize);
        }

        public Task<Contract?> GetByIdAsync(long contractId)
        {
            return BuildBaseQuery()
                .FirstOrDefaultAsync(c => c.Id == contractId);
        }

        public async Task<PagedResult<Contract>> GetByUserIdAsync(Guid userId, int pageNumber, int pageSize)
        {
            var query = BuildBaseQuery()
                .Where(c => c.OwnerId == userId || c.RenterId == userId)
                .OrderByDescending(c => c.SubmittedAt);

            return await CreatePagedResultAsync(query, pageNumber, pageSize);
        }

        public async Task<PagedResult<Contract>> GetByPropertyIdAsync(long propertyId, int pageNumber, int pageSize)
        {
            var query = BuildBaseQuery()
                .Where(c => c.PropertyId == propertyId)
                .OrderByDescending(c => c.SubmittedAt);

            return await CreatePagedResultAsync(query, pageNumber, pageSize);
        }

        public async Task<IEnumerable<Contract>> GetPendingContractsAsync()
        {
            return await BuildBaseQuery()
                .Where(c => c.AnchoringStatus == ContractAnchoringStatus.Pending)
                .ToListAsync();
        }

        public async Task AddAsync(Contract contract)
        {
            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Contract contract)
        {
            _context.Contracts.Update(contract);
            await _context.SaveChangesAsync();
        }

        private IQueryable<Contract> BuildBaseQuery()
        {
            return _context.Contracts
                .Include(c => c.Property)
                .Include(c => c.Renter);
        }

        private static async Task<PagedResult<Contract>> CreatePagedResultAsync(IQueryable<Contract> query, int pageNumber, int pageSize)
        {
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<Contract>
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
