using MARN_API.DTOs.Common;
using MARN_API.DTOs.PropertyFeedback;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IPropertyCommentService
    {
        Task<ServiceResult<PagedResult<PropertyCommentDto>>> GetByPropertyIdAsync(long propertyId, int pageNumber, int pageSize);
        Task<ServiceResult<PropertyComment>> CreateAsync(long propertyId, Guid userId, CreatePropertyCommentDto dto);
        Task<ServiceResult<PropertyComment>> UpdateAsync(long propertyId, long commentId, Guid userId, UpdatePropertyCommentDto dto);
        Task<ServiceResult<bool>> DeleteAsync(long propertyId, long commentId, Guid userId);
    }
}
