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
        public Task<bool> ExistsAsync(long propertyId);
        #endregion


        #region Property Operation
        Task<Property?> GetByIdAsync(long id);
        Task<PropertyDetailsDto?> GetPropertyDetailsAsync(long propertyId, Guid? currentUserId);
        Task<PropertySearchResultDto> SearchPropertiesAsync(PropertySearchFilterDto filter, Guid? currentUserId);
        Task IncrementViewsAsync(long propertyId);
        Task UpdatePropertyAsync(Property property);
        Task AddPropertyAsync(Property property);

        #region Deletion
        Task<List<long>> GetPropertyIdsByOwnerAsync(Guid ownerId);
        Task<List<string>> GetMediaPathsByPropertyIdsAsync(List<long> propertyIds);
        Task DeleteMediaByPropertyIdsAsync(List<long> propertyIds);
        #endregion

        #endregion
    }
}
