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
                        .Where(p => p.Status == PaymentStatus.Pending)
                        .OrderBy(p => p.DueDate)
                        .Select(p => p.Amount)
                        .FirstOrDefault(),

                    PaymentId = c.Payments
                        .Where(p => p.Status == PaymentStatus.Pending)
                        .OrderBy(p => p.DueDate)
                        .Select(p => p.Id)
                        .FirstOrDefault(),

                    // True if there is no overdue unpaid payment (all due payments are succeeded)
                    // IsPaymentMade = !c.Payments
                    //     .Where(p => p.DueDate <= DateTime.UtcNow)
                    //     .Any(p => p.Status != PaymentStatus.Succeeded)
                    IsPaymentMade = c.Payments
                        .Where(p => p.DueDate >= DateTime.UtcNow)
                        .OrderBy(p => p.DueDate)
                        .Select(p => p.Status == PaymentStatus.Succeeded)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }
        #endregion


        #region Owner Dashboard
        public Task<List<Contract>> GetContracts(Guid userId)
        {
            return Context.Contracts
                .Where(c => c.OwnerId == userId)
                .Include(c => c.Property)
                .Include(c => c.Renter)
                .ToListAsync();
        }

        public Task<int> GetOwnedPropertiesOccupiedPlacesCount(Guid userId)
        {
            return Context.Contracts
                .Where(c => c.OwnerId == userId && c.Status == Enums.ContractStatus.Active)
                .Select(c => c.Property.IsShared ? 1 : c.Property.MaxOccupants)
                .SumAsync();
        }
        #endregion
    }
}
