using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Common;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IAdminRoleManagementService
    {
        Task<ServiceResult<List<AdminRoleDefinitionDto>>> GetRolesAsync();
        Task<ServiceResult<PagedResult<AdminRoleUserListItemDto>>> GetUsersAsync(AdminRoleManagementQueryDto query);
        Task<ServiceResult<AdminRoleUserDetailsDto>> GetUserAsync(Guid userId);
        Task<ServiceResult<AdminRoleUserDetailsDto>> UpdateUserRolesAsync(Guid userId, AdminUpdateUserRolesDto request);
    }
}
