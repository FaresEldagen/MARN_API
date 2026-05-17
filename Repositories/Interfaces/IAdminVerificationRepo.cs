using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Common;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IAdminVerificationRepo
    {
        Task<PagedResult<AdminUserVerificationDto>> GetPendingUserVerificationsAsync(int pageNumber, int pageSize);
        Task<AdminUserVerificationDto?> GetUserVerificationDetailsAsync(Guid userId);
        Task<ApplicationUser?> GetUserForVerificationAsync(Guid userId);

        Task<PagedResult<AdminPropertyVerificationDto>> GetPendingPropertyVerificationsAsync(int pageNumber, int pageSize);
        Task<AdminPropertyVerificationDto?> GetPropertyVerificationDetailsAsync(long propertyId);
        Task<Property?> GetPropertyForVerificationAsync(long propertyId);

        Task SaveChangesAsync();
    }
}
