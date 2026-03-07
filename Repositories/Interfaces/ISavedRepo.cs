using MARN_API.Enums;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface ISavedRepo
    {
        public Task<List<Property>> GetSavedProperties(Guid userId);
    }
}
