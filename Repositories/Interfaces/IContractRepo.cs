using MARN_API.DTOs.Dashboard;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IContractRepo
    {
        #region User Dashboard
        public Task<List<ActiveRentalCardDto>> GetActiveRentals(Guid userId);
        #endregion


        #region Owner Dashboard
        public Task<List<OwnerContractCardDto>> GetContracts(Guid userId);
        public Task<List<OwnerContractCardDto>> GetContractsByProperty(Guid userId, long propertyId);
        public Task<int> GetOwnedPropertiesOccupiedPlacesCount(Guid userId);
        #endregion


        public Task<bool> CheackActiveContractsByUserId(Guid userId);
    }
}
