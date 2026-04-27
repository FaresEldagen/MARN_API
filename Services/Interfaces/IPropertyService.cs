using MARN_API.DTOs.Property;
using MARN_API.Models;
using System;
using System.Threading.Tasks;

namespace MARN_API.Services.Interfaces
{
    public interface IPropertyService
    {
        Task<ServiceResult<bool>> AddPropertyAsync(AddPropertyDto dto, Guid userId);
    }
}
