using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Common;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IAdminUserManagementRepo
    {
        Task<PagedResult<AdminUserListItemDto>> GetUsersAsync(AdminUserManagementQueryDto query);
        Task<AdminUserDetailsDto?> GetUserDetailsAsync(Guid userId);
        Task<ApplicationUser?> GetUserByIdAsync(Guid userId);
        Task<bool> IsAdminUserAsync(Guid userId);
        Task<List<string>> GetUserRolesAsync(Guid userId);
        Task SaveChangesAsync();
    }
}
