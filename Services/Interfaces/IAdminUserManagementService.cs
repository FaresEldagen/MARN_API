using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Common;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IAdminUserManagementService
    {
        Task<ServiceResult<PagedResult<AdminUserListItemDto>>> GetUsersAsync(AdminUserManagementQueryDto query);
        Task<ServiceResult<AdminUserDetailsDto>> GetUserDetailsAsync(Guid userId);
        Task<ServiceResult<bool>> BanUserAsync(Guid userId);
        Task<ServiceResult<bool>> UnbanUserAsync(Guid userId);
        Task<ServiceResult<bool>> RestoreDeletedUserAsync(Guid userId);
        Task<ServiceResult<bool>> DeleteUserAsync(Guid userId);
    }
}
