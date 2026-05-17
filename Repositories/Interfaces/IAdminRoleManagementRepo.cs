using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Common;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IAdminRoleManagementRepo
    {
        Task<List<AdminRoleDefinitionDto>> GetRolesAsync();
        Task<PagedResult<AdminRoleUserListItemDto>> GetUsersAsync(AdminRoleManagementQueryDto query);
        Task<AdminRoleUserDetailsDto?> GetUserAsync(Guid userId);
        Task<ApplicationUser?> GetUserEntityAsync(Guid userId);
        Task<List<string>> GetUserRolesAsync(Guid userId);
        Task<int> GetAdminsCountAsync();
    }
}
