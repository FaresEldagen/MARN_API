using MARN_API.DTOs.Common;
using MARN_API.DTOs.Dashboard;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IContractRepo
    {
        #region Dashboards
        public Task<List<ActiveRentalCardDto>> GetActiveRentals(Guid userId);
        public Task<List<OwnerContractCardDto>> GetOwnerContracts(Guid userId);
        public Task<List<RenterContractCardDto>> GetRenterContracts(Guid userId);
        public Task<List<OwnerContractCardDto>> GetContractsByProperty(Guid userId, long propertyId);
        public Task<int> GetOwnedPropertiesOccupiedPlacesCount(Guid userId);
        #endregion


        #region Checks
        public Task<bool> HasActiveContractsAsync(Guid userId);
        public Task<bool> HasEligiblePropertyContractAsync(Guid userId, long propertyId);
        public Task<bool> HasActiveContractsForPropertyAsync(long propertyId);
        #endregion


        #region Contract Operations
        Task AddAsync(Contract contract);
        Task<Contract?> GetByIdAsync(long contractId);
        Task<IEnumerable<Contract>> GetPendingContractsAsync();
        Task SignContractAsync(Contract contract);
        Task UpdateAsync(Contract contract);
        Task DeleteAsync(Contract contract);
        #endregion


        #region Admin Operations
        Task<PagedResult<Contract>> GetAllAsync(int pageNumber, int pageSize);
        Task<PagedResult<Contract>> GetByUserIdAsync(Guid userId, int pageNumber, int pageSize);
        Task<PagedResult<Contract>> GetByPropertyIdAsync(long propertyId, int pageNumber, int pageSize);
        #endregion
    }
}
