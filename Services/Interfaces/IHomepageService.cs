using MARN_API.DTOs.Property;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IHomepageService
    {
        Task<ServiceResult<PropertySearchResultDto>> GetRecommendedPropertiesAsync(Guid? userId);
    }
}
