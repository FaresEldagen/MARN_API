using MARN_API.Data;
using MARN_API.DTOs.Common;
using MARN_API.DTOs.Dashboard;
using MARN_API.Enums;
using MARN_API.Enums.Payment;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class ContractRepo : IContractRepo
    {
        private readonly AppDbContext Context;

        public ContractRepo(AppDbContext context)
        {
            Context = context;
        }



        #region Dashboards
        public Task<List<ActiveRentalCardDto>> GetActiveRentals(Guid userId)
        {
            var now = DateTime.UtcNow;

            return Context.Contracts
                .AsNoTracking()
                .Where(c => c.RenterId == userId && c.Status == ContractStatus.Active)
                .Select(c => new ActiveRentalCardDto
                {
                    ContractId = c.Id,
                    ContractStatus = c.Status,
                    StartDate = c.LeaseStartDate.ToDateTime(TimeOnly.MinValue),
                    EndDate = c.LeaseEndDate.ToDateTime(TimeOnly.MinValue),

                    PropertyTitle = c.Property.Title,
                    PropertyAddress = c.Property.Address,

                    PropertyImageUrl = c.Property.Media
                        .Where(m => m.IsPrimary)
                        .Select(m => m.Path)
                        .FirstOrDefault() ?? "",

                    PaymentFrequency = c.PaymentFrequency,

                    NextPaymentAmount = Context.PaymentSchedules
                        .Where(p => p.ContractId == c.Id && p.DueDate - now <= TimeSpan.FromDays(7))
                        .OrderBy(p => p.DueDate)
                        .Select(p => (decimal?)p.Amount)
                        .FirstOrDefault()
                        ?? Context.PaymentSchedules
                            .Where(p => p.ContractId == c.Id)
                            .OrderByDescending(p => p.DueDate)
                            .Select(p => (decimal?)p.Amount)
                            .FirstOrDefault()
                        ?? 0m,

                    PaymentId = Context.PaymentSchedules
                        .Where(p => p.ContractId == c.Id && p.DueDate - now <= TimeSpan.FromDays(7))
                        .OrderBy(p => p.DueDate)
                        .Select(p => (long?)p.Id)
                        .FirstOrDefault()
                        ?? Context.PaymentSchedules
                            .Where(p => p.ContractId == c.Id)
                            .OrderByDescending(p => p.DueDate)
                            .Select(p => (long?)p.Id)
                            .FirstOrDefault()
                        ?? 0L,

                    IsPaymentMade = Context.PaymentSchedules
                        .Where(p => p.ContractId == c.Id && p.DueDate - now <= TimeSpan.FromDays(7))
                        .OrderBy(p => p.DueDate)
                        .Select(p => (bool?)(p.Status == PaymentScheduleStatus.PaidEarly || p.Status == PaymentScheduleStatus.PaidOnTime || p.Status == PaymentScheduleStatus.PaidLate))
                        .FirstOrDefault()
                        ?? Context.PaymentSchedules
                            .Where(p => p.ContractId == c.Id)
                            .OrderByDescending(p => p.DueDate)
                            .Select(p => (bool?)(p.Status == PaymentScheduleStatus.PaidEarly || p.Status == PaymentScheduleStatus.PaidOnTime || p.Status == PaymentScheduleStatus.PaidLate))
                            .FirstOrDefault()
                        ?? false
                })
                .ToListAsync();
        }

        public Task<List<OwnerContractCardDto>> GetOwnerContracts(Guid userId)
        {
            return Context.Contracts
                .AsNoTracking()
                .Where(c => c.Property.OwnerId == userId)
                .OrderByDescending(c => c.LeaseEndDate)
                .Select(c => new OwnerContractCardDto
                {
                    ContractId = c.Id,
                    ContractStatus = c.Status,
                    ExpiryDate = c.LeaseEndDate.ToDateTime(TimeOnly.MinValue),

                    PropertyId = c.PropertyId,
                    PropertyTitle = c.Property.Title,

                    RenterId = c.RenterId,
                    RenterName = $"{c.Renter.FirstName} {c.Renter.LastName}"
                })          
                .ToListAsync();
        }

        public Task<List<RenterContractCardDto>> GetRenterContracts(Guid userId)
        {
            return Context.Contracts
                .AsNoTracking()
                .Where(c => c.RenterId == userId)
                .OrderByDescending(c => c.LeaseEndDate)
                .Select(c => new RenterContractCardDto
                {
                    ContractId = c.Id,
                    ContractStatus = c.Status,
                    ExpiryDate = c.LeaseEndDate.ToDateTime(TimeOnly.MinValue),

                    OwnerId = c.Property.OwnerId,
                    OwnerName = $"{c.Property.Owner.FirstName} {c.Property.Owner.LastName}",

                    PropertyId = c.PropertyId,
                    PropertyTitle = c.Property.Title
                })          
                .ToListAsync();
        }

        public Task<List<OwnerContractCardDto>> GetContractsByProperty(Guid userId, long propertyId)
        {
            return Context.Contracts
                .AsNoTracking()
                .Where(c => c.Property.OwnerId == userId && c.PropertyId == propertyId)
                .OrderByDescending(c => c.LeaseEndDate)
                .Select(c => new OwnerContractCardDto
                {
                    ContractId = c.Id,
                    ContractStatus = c.Status,
                    ExpiryDate = c.LeaseEndDate.ToDateTime(TimeOnly.MinValue),

                    PropertyId = c.PropertyId,
                    PropertyTitle = c.Property.Title,

                    RenterId = c.RenterId,
                    RenterName = $"{c.Renter.FirstName} {c.Renter.LastName}"
                })
                .ToListAsync();
        }

        public Task<int> GetOwnedPropertiesOccupiedPlacesCount(Guid userId)
        {
            return Context.Contracts
                .Where(c => c.Property.OwnerId == userId && c.Status == ContractStatus.Active)
                .Select(c => c.Property.IsShared ? 1 : c.Property.MaxOccupants)
                .SumAsync();
        }
        #endregion


        #region Checks
        public async Task<bool> HasActiveContractsAsync(Guid userId)
        {
            bool isRenterWithActiveContract = await Context.Contracts
                .AsNoTracking()
                .AnyAsync(c => c.RenterId == userId && c.Status == ContractStatus.Active);
            bool isOwnerWithActiveContract = await Context.Contracts
                .AsNoTracking()
                .AnyAsync(c => c.Property.OwnerId == userId && c.Status == ContractStatus.Active);

            return isRenterWithActiveContract || isOwnerWithActiveContract;
        }

        public Task<bool> HasEligiblePropertyContractAsync(Guid userId, long propertyId)
        {
            return Context.Contracts
                .AsNoTracking()
                .AnyAsync(c =>
                    c.RenterId == userId &&
                    c.PropertyId == propertyId &&
                    (c.Status == ContractStatus.Active || c.Status == ContractStatus.Expired));
        }

        public Task<bool> HasActiveContractsForPropertyAsync(long propertyId)
        {
            return Context.Contracts
                .AsNoTracking()
                .AnyAsync(c => c.PropertyId == propertyId && c.Status == ContractStatus.Active);
        }
        #endregion


        #region Contract Operations
        public async Task AddAsync(Contract contract)
        {
            Context.Contracts.Add(contract);
            await Context.SaveChangesAsync();
        }

        public Task<Contract?> GetByIdAsync(long contractId)
        {
            return Context.Contracts
                .Include(c => c.Property)
                .FirstOrDefaultAsync(c => c.Id == contractId);
        }

        public async Task<IEnumerable<Contract>> GetPendingContractsAsync()
        {
            return await Context.Contracts
                .Where(c => c.AnchoringStatus == ContractAnchoringStatus.Pending)
                .ToListAsync();
        }

        public async Task UpdateAsync(Contract contract)
        {
            Context.Contracts.Update(contract);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Contract contract)
        {
            Context.Contracts.Remove(contract);
            await Context.SaveChangesAsync();
        }
        #endregion


        #region Admin Operations
        public async Task<PagedResult<Contract>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = Context.Contracts
                .Include(c => c.Property)
                .Include(c => c.Renter)
                .OrderByDescending(c => c.CreatedAt);

            return await CreatePagedResultAsync(query, pageNumber, pageSize);
        }

        public async Task<PagedResult<Contract>> GetByUserIdAsync(Guid userId, int pageNumber, int pageSize)
        {
            var query = Context.Contracts
                .Include(c => c.Property)
                .Include(c => c.Renter)
                .Where(c => c.Property.OwnerId == userId || c.RenterId == userId)
                .OrderByDescending(c => c.CreatedAt);

            return await CreatePagedResultAsync(query, pageNumber, pageSize);
        }

        public async Task<PagedResult<Contract>> GetByPropertyIdAsync(long propertyId, int pageNumber, int pageSize)
        {
            var query = Context.Contracts
                .Include(c => c.Property)
                .Include(c => c.Renter)
                .Where(c => c.PropertyId == propertyId)
                .OrderByDescending(c => c.CreatedAt);

            return await CreatePagedResultAsync(query, pageNumber, pageSize);
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
        #endregion
    }
}
