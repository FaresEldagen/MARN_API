using MARN_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MARN_API.Repositories.Interfaces
{
    public interface IPropertyAmenityRepo
    {
        Task<List<PropertyAmenity>> GetByPropertyIdAsync(long propertyId);
        Task AddByPropertyIdAsync(long propertyId, PropertyAmenity amenity);
        Task RemoveByObjectIdAsync(long id);
    }
}
