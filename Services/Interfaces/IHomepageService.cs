using MARN_API.DTOs.Property;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IHomepageService
    {
        Task<ServiceResult<List<PropertyCardDto>>> GetRecommendedPropertiesAsync(Guid? userId);
    }
}
