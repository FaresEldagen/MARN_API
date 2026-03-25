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
            return Context.Contracts
                .AsNoTracking()
                .Where(c => c.RenterId == userId && c.Status == ContractStatus.Active)
                .Select(c => new ActiveRentalCardDto
                {
                    ContractId = c.Id,
                    ContractStatus = c.Status,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,

                    PropertyTitle = c.Property.Title,
                    PropertyAddress = c.Property.Address,

                    PropertyImageUrl = c.Property.Media
                        .Where(m => m.IsPrimary)
                        .Select(m => m.Path)
                        .FirstOrDefault() ?? "",

                    PaymentFrequency = c.PaymentFrequency,

                    // Next pending payment for this contract (if any)
                    NextPaymentAmount = c.Payments
                        .Where(p => p.DueDate >= DateTime.UtcNow)
                        .OrderBy(p => p.DueDate)
                        .Select(p => p.TotalAmount)
                        .FirstOrDefault(),

                    PaymentId = c.Payments
                        .Where(p => p.DueDate >= DateTime.UtcNow)
                        .OrderBy(p => p.DueDate)
                        .Select(p => p.Id)
                        .FirstOrDefault(),

                    IsPaymentMade = c.Payments
                        .Where(p => p.DueDate >= DateTime.UtcNow)
                        .OrderBy(p => p.DueDate)
                        .Select(p => p.Status == PaymentStatus.Succeeded)
                        .FirstOrDefault()

                    // True if there is no overdue unpaid payment (all due payments are succeeded)
                    // IsPaymentMade = !c.Payments
                    //     .Where(p => p.DueDate <= DateTime.UtcNow)
                    //     .Any(p => p.Status != PaymentStatus.Succeeded)
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
                    ExpiryDate = c.EndDate,

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
    }
}
