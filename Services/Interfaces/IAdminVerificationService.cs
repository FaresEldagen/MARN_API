using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Common;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IAdminVerificationService
    {
        Task<ServiceResult<PagedResult<AdminUserVerificationDto>>> GetPendingUserVerificationsAsync(AdminVerificationQueryDto query);
        Task<ServiceResult<AdminUserVerificationDto>> GetUserVerificationDetailsAsync(Guid userId);
        Task<ServiceResult<bool>> ApproveUserVerificationAsync(Guid userId);
        Task<ServiceResult<bool>> DeclineUserVerificationAsync(Guid userId, AdminVerificationDecisionDto decision);

        Task<ServiceResult<PagedResult<AdminPropertyVerificationDto>>> GetPendingPropertyVerificationsAsync(AdminVerificationQueryDto query);
        Task<ServiceResult<AdminPropertyVerificationDto>> GetPropertyVerificationDetailsAsync(long propertyId);
        Task<ServiceResult<bool>> ApprovePropertyVerificationAsync(long propertyId);
        Task<ServiceResult<bool>> DeclinePropertyVerificationAsync(long propertyId, AdminVerificationDecisionDto decision);
    }
}
