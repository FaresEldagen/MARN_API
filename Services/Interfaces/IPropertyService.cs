using MARN_API.DTOs.Property;
using MARN_API.Models;
using System;
using System.Threading.Tasks;

namespace MARN_API.Services.Interfaces
{
    public interface IPropertyService
    {
        Task<ServiceResult<bool>> AddPropertyAsync(AddPropertyDto dto, Guid userId);
        Task<ServiceResult<PropertySearchResultDto>> SearchPropertiesAsync(PropertySearchFilterDto filter, Guid? userId);
        Task<ServiceResult<PropertyDetailsDto>> GetPropertyDetailsAsync(long propertyId, Guid? userId);
        Task<ServiceResult<PropertyEditDataDto>> GetPropertyEditAsync(long propertyId, Guid userId);
        Task<ServiceResult<bool>> EditPropertyAsync(long propertyId, EditPropertyDto dto, Guid userId);
        Task<ServiceResult<bool>> ToggleSavePropertyAsync(long propertyId, Guid userId);
        Task<ServiceResult<bool>> DeactivatePropertyAsync(long propertyId, Guid userId);
        Task<ServiceResult<bool>> DeletePropertyAsync(long propertyId, Guid userId);
    }
}
