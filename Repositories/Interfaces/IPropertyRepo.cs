using MARN_API.DTOs.Dashboard;
using MARN_API.DTOs.Property;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IPropertyRepo
    {
        #region Owner Dashboard and Profile
        public Task<List<OwnerDashboardPropertyCardDto>> GetOwnerDashboardProperties(Guid userId);
        public Task<List<PropertyCardDto>> GetOwnerProfileProperties(Guid userId);
        public Task<int> GetOwnedPropertiesViewsCount(Guid userId);
        public Task<int> GetOwnedPropertiesPlacesCount(Guid userId);
        public Task<float> GetOwnerAverageRating(Guid userid);
        public Task<int> GetOwnerRatingsCount(Guid userId);
        #endregion
    }
}
