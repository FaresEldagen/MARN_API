using MARN_API.DTOs.PropertyFeedback;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IPropertyRatingRepo
    {
        Task<PropertyRatingSummaryDto> GetSummaryAsync(long propertyId, Guid? currentUserId = null);
        Task<PropertyRating?> GetByPropertyAndUserAsync(long propertyId, Guid userId);
        Task<bool> ExistsAsync(long propertyId, Guid userId);
        Task<PropertyRating> CreateAsync(PropertyRating rating);
        Task<PropertyRating> UpdateAsync(PropertyRating rating);
        Task DeleteAsync(PropertyRating rating);
        Task DeleteByUserIdAsync(Guid userId);
        Task DeleteByPropertyIdAsync(long propertyId);
    }
}
