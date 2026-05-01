using MARN_API.DTOs.PropertyFeedback;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IPropertyRatingService
    {
        Task<ServiceResult<PropertyRatingSummaryDto>> GetSummaryAsync(long propertyId, Guid? currentUserId = null);
        Task<ServiceResult<PropertyRatingDto>> CreateAsync(long propertyId, Guid userId, CreatePropertyRatingDto dto);
        Task<ServiceResult<PropertyRatingDto>> UpdateAsync(long propertyId, Guid userId, UpdatePropertyRatingDto dto);
        Task<ServiceResult<bool>> DeleteAsync(long propertyId, Guid userId);
    }
}
