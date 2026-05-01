using MARN_API.DTOs.Common;
using MARN_API.DTOs.PropertyFeedback;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IPropertyCommentRepo
    {
        Task<PagedResult<PropertyCommentDto>> GetByPropertyIdAsync(long propertyId, int pageNumber, int pageSize);
        Task<PropertyComment?> GetByIdAsync(long commentId);
        Task<PropertyComment> CreateAsync(PropertyComment comment);
        Task<PropertyComment> UpdateAsync(PropertyComment comment);
        Task DeleteAsync(PropertyComment comment);
        Task DeleteByUserIdAsync(Guid userId);
        Task DeleteByPropertyIdAsync(long propertyId);
    }
}
