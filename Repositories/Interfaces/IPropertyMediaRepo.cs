using MARN_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MARN_API.Repositories.Interfaces
{
    public interface IPropertyMediaRepo
    {
        Task<List<PropertyMedia>> GetByPropertyIdAsync(long propertyId);
        Task AddByPropertyIdAsync(long propertyId, PropertyMedia media);
        Task RemoveByObjectIdAsync(long id);
    }
}
