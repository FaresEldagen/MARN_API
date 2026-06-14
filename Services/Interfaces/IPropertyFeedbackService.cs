using MARN_API.DTOs.PropertyFeedback;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IPropertyFeedbackService
    {
        Task<ServiceResult<PropertyFeedbackSummaryDto>> GetAsync(long propertyId, Guid? currentUserId, int pageNumber, int pageSize);
        Task<ServiceResult<PropertyFeedbackDto>> CreateAsync(long propertyId, Guid userId, PropertyFeedbackRequestDto dto);
        Task<ServiceResult<PropertyFeedbackDto>> UpdateAsync(long propertyId, Guid userId, PropertyFeedbackRequestDto dto);
        Task<ServiceResult<bool>> DeleteAsync(long propertyId, Guid userId);
    }
}
