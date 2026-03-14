using MARN_API.DTOs.Property;
using MARN_API.Enums;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface ISavedPropertyRepo
    {
        public Task<List<PropertyCardDto>> GetSavedProperties(Guid userId);
    }
}
