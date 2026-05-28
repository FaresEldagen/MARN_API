using MARN_API.DTOs.Common;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface ICsvSeedImportService
    {
        Task<ServiceResult<CsvSeedImportResultDto>> ImportPropertiesAsync(Microsoft.AspNetCore.Http.IFormFile file);
        Task<ServiceResult<CsvSeedImportResultDto>> ImportUsersAsync(Microsoft.AspNetCore.Http.IFormFile file);
        Task<ServiceResult<CsvSeedImportResultDto>> ImportUserActivitiesAsync(Microsoft.AspNetCore.Http.IFormFile file);
    }
}
