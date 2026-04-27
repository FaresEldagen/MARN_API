using MARN_API.Data;
using MARN_API.DTOs.Dashboard;
using MARN_API.Enums;
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


        #region User Dashboard
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
                    StartDate = c.LeaseStartDate.HasValue
                        ? c.LeaseStartDate.Value.ToDateTime(TimeOnly.MinValue)
                        : c.SubmittedAt,
                    EndDate = c.LeaseEndDate.HasValue
                        ? c.LeaseEndDate.Value.ToDateTime(TimeOnly.MinValue)
                        : c.SubmittedAt,

                    PropertyTitle = c.Property.Title,
                    PropertyAddress = c.Property.Address,

                    PropertyImageUrl = c.Property.Media
                        .Where(m => m.IsPrimary)
                        .Select(m => m.Path)
                        .FirstOrDefault() ?? "",

                    PaymentFrequency = c.PaymentFrequency,

                    NextPaymentAmount = Context.Payments
                        .Where(p => p.ContractId == c.Id && p.DueDate >= now)
                        .OrderBy(p => p.DueDate)
                        .Select(p => (decimal?)p.AmountTotal)
                        .FirstOrDefault()
                        ?? Context.Payments
                            .Where(p => p.ContractId == c.Id)
                            .OrderByDescending(p => p.DueDate)
                            .Select(p => (decimal?)p.AmountTotal)
                            .FirstOrDefault()
                        ?? 0m,

                    PaymentId = Context.Payments
                        .Where(p => p.ContractId == c.Id && p.DueDate >= now)
                        .OrderBy(p => p.DueDate)
                        .Select(p => (long?)p.Id)
                        .FirstOrDefault()
                        ?? Context.Payments
                            .Where(p => p.ContractId == c.Id)
                            .OrderByDescending(p => p.DueDate)
                            .Select(p => (long?)p.Id)
                            .FirstOrDefault()
                        ?? 0L,

                    IsPaymentMade = Context.Payments
                        .Where(p => p.ContractId == c.Id && p.DueDate >= now)
                        .OrderBy(p => p.DueDate)
                        .Select(p => (bool?)(p.Status == PaymentStatus.Succeeded))
                        .FirstOrDefault()
                        ?? Context.Payments
                            .Where(p => p.ContractId == c.Id)
                            .OrderByDescending(p => p.DueDate)
                            .Select(p => (bool?)(p.Status == PaymentStatus.Succeeded))
                            .FirstOrDefault()
                        ?? false
                })
                .ToListAsync();
        }
        #endregion


        #region Owner Dashboard
        public Task<List<OwnerContractCardDto>> GetContracts(Guid userId)
        {
            return Context.Contracts
                .AsNoTracking()
                .Where(c => c.OwnerId == userId)
                .Select(c => new OwnerContractCardDto
                {
                    ContractId = c.Id,
                    ContractStatus = c.Status,
                    ExpiryDate = c.LeaseEndDate.HasValue
                        ? c.LeaseEndDate.Value.ToDateTime(TimeOnly.MinValue)
                        : c.SubmittedAt,

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
                .Where(c => c.OwnerId == userId && c.Status == ContractStatus.Active)
                .Select(c => c.Property.IsShared ? 1 : c.Property.MaxOccupants)
                .SumAsync();
        }
        #endregion


        public async Task<bool> CheackActiveContractsByUserId(Guid userId)
        {
            bool isRenterWithActiveContract = await Context.Contracts
                .AsNoTracking()
                .AnyAsync(c => c.RenterId == userId && c.Status == ContractStatus.Active);
            bool isOwnerWithActiveContract = await Context.Contracts
                .AsNoTracking()
                .AnyAsync(c => c.OwnerId == userId && c.Status == ContractStatus.Active);

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
    }
}
