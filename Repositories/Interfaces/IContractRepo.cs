using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IContractRepo
    {
        #region User Dashboard
        public Task<List<Contract>> GetActiveRentals(Guid userId);
        #endregion


        #region Owner Dashboard
        public Task<List<Contract>> GetContracts(Guid userId);
        public Task<int> GetOwnedPropertiesOccupiedPlacesCount(Guid userId);
        #endregion
    }
}
