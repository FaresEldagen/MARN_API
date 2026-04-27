using MARN_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MARN_API.Repositories.Interfaces
{
    public interface IPropertyRuleRepo
    {
        Task<List<PropertyRule>> GetByPropertyIdAsync(long propertyId);
        Task AddByPropertyIdAsync(long propertyId, PropertyRule rule);
        Task RemoveByObjectIdAsync(long id);
    }
}
