using MARN_API.Data;
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
        public Task<List<Contract>> GetActiveRentals(Guid userId)
        {
            return Context.Contracts
                .Where(c => c.RenterId == userId && c.Status == Enums.ContractStatus.Active)
                .Include(c => c.Property)
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
