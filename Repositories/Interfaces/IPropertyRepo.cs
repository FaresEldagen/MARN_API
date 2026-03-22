using MARN_API.DTOs.Dashboard;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IPropertyRepo
    {
        public Task<List<OwnerPropertyCardDto>> GetOwnedProperties(Guid userId);
        public Task<int> GetOwnedPropertiesViewsCount(Guid userId);
        public Task<int> GetOwnedPropertiesPlacesCount(Guid userId);
    }
}
