using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IExternalPropertyAiClient
    {
        Task<ExternalRecommendationResult> GetRecommendedPropertyIdsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task NotifyPropertyAddedAsync(long propertyId, CancellationToken cancellationToken = default);
        Task NotifyPropertyUpdatedAsync(long propertyId, CancellationToken cancellationToken = default);
        Task NotifyPropertyDeletedAsync(long propertyId, CancellationToken cancellationToken = default);
    }
}
