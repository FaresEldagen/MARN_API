using MARN_API.DTOs.PropertyFeedback;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IPropertyFeedbackRepo
    {
        Task<PropertyFeedbackSummaryDto> GetSummaryAsync(long propertyId, Guid? currentUserId, int pageNumber, int pageSize);
        Task<PropertyFeedback?> GetByPropertyAndUserAsync(long propertyId, Guid userId);
        Task<PropertyFeedback?> GetByIdAsync(long feedbackId);
        Task<PropertyFeedback> CreateAsync(PropertyFeedback feedback);
        Task<PropertyFeedback> UpdateAsync(PropertyFeedback feedback);
        Task DeleteAsync(PropertyFeedback feedback);
        Task DeleteByUserIdAsync(Guid userId);
        Task DeleteByPropertyIdAsync(long propertyId);
    }
}
