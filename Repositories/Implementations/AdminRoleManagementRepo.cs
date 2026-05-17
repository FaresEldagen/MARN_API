using MARN_API.Data;
using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Common;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class AdminRoleManagementRepo : IAdminRoleManagementRepo
    {
        private const string AdminRoleNormalizedName = "ADMIN";
        private const string RenterRoleNormalizedName = "RENTER";
        private const string OwnerRoleNormalizedName = "OWNER";
        private readonly AppDbContext _context;

        public AdminRoleManagementRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AdminRoleDefinitionDto>> GetRolesAsync()
        {
            return await _context.Roles
                .AsNoTracking()
                .OrderBy(r => r.Name)
                .Select(r => new AdminRoleDefinitionDto
                {
                    RoleName = r.Name ?? string.Empty,
                    NormalizedName = r.NormalizedName ?? string.Empty,
                    UsersCount = _context.UserRoles.Count(ur => ur.RoleId == r.Id),
                    IsProtected = r.NormalizedName == RenterRoleNormalizedName || r.NormalizedName == OwnerRoleNormalizedName,
                    IsAssignable = r.NormalizedName != RenterRoleNormalizedName && r.NormalizedName != OwnerRoleNormalizedName
                })
                .ToListAsync();
        }

        public async Task<PagedResult<AdminRoleUserListItemDto>> GetUsersAsync(AdminRoleManagementQueryDto query)
        {
            var usersQuery = BuildUsersQuery(query);

            var totalCount = await usersQuery.CountAsync();
            var users = await usersQuery
                .OrderByDescending(u => u.CreatedAt)
                .ThenBy(u => u.Id)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(u => new AdminRoleUserListItemDto
                {
                    UserId = u.Id,
                    FullName = (u.FirstName + " " + u.LastName).Trim(),
                    Email = u.Email,
                    ProfileImage = u.ProfileImage,
                    AccountStatus = u.AccountStatus,
                    IsDeleted = u.DeletedAt != null,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            var roleLookup = await GetRolesLookupAsync(users.Select(u => u.UserId).ToList());
            foreach (var user in users)
            {
                user.Roles = roleLookup.TryGetValue(user.UserId, out var roles)
                    ? roles
                    : [];
            }

            return new PagedResult<AdminRoleUserListItemDto>
            {
                Items = users,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)query.PageSize)
            };
        }

        public async Task<AdminRoleUserDetailsDto?> GetUserAsync(Guid userId)
        {
            var user = await AllUsers()
                .Where(u => u.Id == userId)
                .Select(u => new AdminRoleUserDetailsDto
                {
                    UserId = u.Id,
                    FullName = (u.FirstName + " " + u.LastName).Trim(),
                    Email = u.Email,
                    UserName = u.UserName,
                    ProfileImage = u.ProfileImage,
                    AccountStatus = u.AccountStatus,
                    IsDeleted = u.DeletedAt != null,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return null;

            user.Roles = await GetUserRolesAsync(userId);
            return user;
        }

        public Task<ApplicationUser?> GetUserEntityAsync(Guid userId)
        {
            return AllUsers().FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<string>> GetUserRolesAsync(Guid userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(
                    _context.Roles,
                    ur => ur.RoleId,
                    r => r.Id,
                    (_, r) => r.Name!)
                .OrderBy(name => name)
                .ToListAsync();
        }

        public Task<int> GetAdminsCountAsync()
        {
            var adminRoleIds = _context.Roles
                .Where(r => r.NormalizedName == AdminRoleNormalizedName)
                .Select(r => r.Id);

            return _context.UserRoles
                .Where(ur => adminRoleIds.Contains(ur.RoleId))
                .Select(ur => ur.UserId)
                .Distinct()
                .CountAsync();
        }

        private IQueryable<ApplicationUser> BuildUsersQuery(AdminRoleManagementQueryDto query)
        {
            var usersQuery = AllUsers();

            if (!query.IncludeDeleted)
            {
                usersQuery = usersQuery.Where(u => u.DeletedAt == null);
            }

            if (query.AccountStatus.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.AccountStatus == query.AccountStatus.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.Role))
            {
                var normalizedRole = query.Role.Trim().ToUpperInvariant();
                var roleIds = _context.Roles
                    .Where(r => r.NormalizedName == normalizedRole)
                    .Select(r => r.Id);

                usersQuery = usersQuery.Where(u =>
                    _context.UserRoles.Any(ur => ur.UserId == u.Id && roleIds.Contains(ur.RoleId)));
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();
                usersQuery = usersQuery.Where(u =>
                    (u.FirstName + " " + u.LastName).ToLower().Contains(search) ||
                    (u.Email != null && u.Email.ToLower().Contains(search)) ||
                    (u.UserName != null && u.UserName.ToLower().Contains(search)));
            }

            return usersQuery;
        }

        private IQueryable<ApplicationUser> AllUsers()
        {
            return _context.Users
                .IgnoreQueryFilters();
        }

        private async Task<Dictionary<Guid, List<string>>> GetRolesLookupAsync(List<Guid> userIds)
        {
            if (userIds.Count == 0)
                return [];

            var roles = await _context.UserRoles
                .Where(ur => userIds.Contains(ur.UserId))
                .Join(
                    _context.Roles,
                    ur => ur.RoleId,
                    r => r.Id,
                    (ur, r) => new { ur.UserId, RoleName = r.Name! })
                .ToListAsync();

            return roles
                .GroupBy(x => x.UserId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.RoleName).OrderBy(name => name).ToList());
        }
    }
}
