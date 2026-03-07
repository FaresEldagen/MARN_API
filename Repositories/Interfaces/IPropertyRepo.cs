using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IPropertyRepo
    {
        public Task<List<Property>> GetOwnedProperties(Guid userId);
        public Task<int> GetOwnedPropertiesViewsCount(Guid userId);
        public Task<int> GetOwnedPropertiesPlacesCount(Guid userId);
    }
}
