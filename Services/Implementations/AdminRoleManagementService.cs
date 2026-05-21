using MARN_API.Data;
using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Common;
using MARN_API.Enums;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MARN_API.Services.Implementations
{
    public class AdminRoleManagementService : IAdminRoleManagementService
    {
        private const int MaxPageSize = 100;
        private const string AdminRoleName = "Admin";
        private const string OwnerRoleName = "Owner";
        private const string RenterRoleName = "Renter";

        private readonly IAdminRoleManagementRepo _roleManagementRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<AdminRoleManagementService> _logger;

        public AdminRoleManagementService(
            IAdminRoleManagementRepo roleManagementRepo,
            UserManager<ApplicationUser> userManager,
            AppDbContext context,
            ILogger<AdminRoleManagementService> logger)
        {
            _roleManagementRepo = roleManagementRepo;
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public async Task<ServiceResult<List<AdminRoleDefinitionDto>>> GetRolesAsync()
        {
            var roles = await _roleManagementRepo.GetRolesAsync();
            return ServiceResult<List<AdminRoleDefinitionDto>>.Ok(roles);
        }

        public async Task<ServiceResult<PagedResult<AdminRoleUserListItemDto>>> GetUsersAsync(AdminRoleManagementQueryDto query)
        {
            NormalizePaging(query);
            var result = await _roleManagementRepo.GetUsersAsync(query);
            return ServiceResult<PagedResult<AdminRoleUserListItemDto>>.Ok(result);
        }

        public async Task<ServiceResult<AdminRoleUserDetailsDto>> GetUserAsync(Guid userId)
        {
            var user = await _roleManagementRepo.GetUserAsync(userId);
            if (user == null)
                return ServiceResult<AdminRoleUserDetailsDto>.Fail("User not found.", resultType: ServiceResultType.NotFound);

            user.AvailableRoles = await _roleManagementRepo.GetRolesAsync();
            return ServiceResult<AdminRoleUserDetailsDto>.Ok(user);
        }

        public async Task<ServiceResult<AdminRoleUserDetailsDto>> UpdateUserRolesAsync(Guid userId, AdminUpdateUserRolesDto request)
        {
            var user = await _roleManagementRepo.GetUserEntityAsync(userId);
            if (user == null)
                return ServiceResult<AdminRoleUserDetailsDto>.Fail("User not found.", resultType: ServiceResultType.NotFound);

            if (user.DeletedAt != null)
                return ServiceResult<AdminRoleUserDetailsDto>.Fail("Deleted users cannot be edited.", resultType: ServiceResultType.Conflict);

            var requestedRoles = (request.Roles ?? [])
                .Where(role => !string.IsNullOrWhiteSpace(role))
                .Select(role => role.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var availableRoles = await _roleManagementRepo.GetRolesAsync();
            var assignableRoles = availableRoles
                .Where(r => r.IsAssignable)
                .ToDictionary(r => r.NormalizedName, r => r.RoleName, StringComparer.OrdinalIgnoreCase);

            var currentRoles = await _userManager.GetRolesAsync(user);
            var protectedRoles = currentRoles
                .Where(role => string.Equals(role, OwnerRoleName, StringComparison.OrdinalIgnoreCase) ||
                               string.Equals(role, RenterRoleName, StringComparison.OrdinalIgnoreCase))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var requestedManagedRoles = requestedRoles
                .Where(role => !string.Equals(role, OwnerRoleName, StringComparison.OrdinalIgnoreCase) &&
                               !string.Equals(role, RenterRoleName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (protectedRoles.Count == 0 && requestedManagedRoles.Count == 0)
            {
                return ServiceResult<AdminRoleUserDetailsDto>.Fail(
                    "At least one role must remain assigned.",
                    resultType: ServiceResultType.BadRequest);
            }

            var invalidRoles = requestedManagedRoles
                .Where(role => !assignableRoles.ContainsKey(role.ToUpperInvariant()))
                .OrderBy(role => role)
                .ToList();

            if (invalidRoles.Count > 0)
            {
                return ServiceResult<AdminRoleUserDetailsDto>.Fail(
                    "Unknown or protected role(s): {0}.",
                    resultType: ServiceResultType.BadRequest,
                    code: "ADMIN_ROLE_UNKNOWN_OR_PROTECTED",
                    messageArguments: [string.Join(", ", invalidRoles)]);
            }

            var normalizedRequestedRoles = requestedManagedRoles
                .Select(role => assignableRoles[role.ToUpperInvariant()])
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Concat(protectedRoles)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            try
            {
                currentRoles = await _userManager.GetRolesAsync(user);
                var userIsCurrentlyAdmin = currentRoles.Any(role => string.Equals(role, AdminRoleName, StringComparison.OrdinalIgnoreCase));
                var willRemainAdmin = normalizedRequestedRoles.Any(role => string.Equals(role, AdminRoleName, StringComparison.OrdinalIgnoreCase));

                if (userIsCurrentlyAdmin && !willRemainAdmin)
                {
                    var adminsCount = await _roleManagementRepo.GetAdminsCountAsync();
                    if (adminsCount <= 1)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<AdminRoleUserDetailsDto>.Fail(
                            "The last admin cannot be removed.",
                            resultType: ServiceResultType.Conflict);
                    }
                }

                var rolesToRemove = currentRoles
                    .Where(role => !normalizedRequestedRoles.Contains(role, StringComparer.OrdinalIgnoreCase))
                    .ToList();

                var rolesToAdd = normalizedRequestedRoles
                    .Where(role => !currentRoles.Contains(role, StringComparer.OrdinalIgnoreCase))
                    .ToList();

                if (rolesToRemove.Count > 0)
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!removeResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<AdminRoleUserDetailsDto>.Fail(
                            "Failed to remove roles.",
                            removeResult.Errors.Select(e => e.Description).ToList(),
                            resultType: ServiceResultType.BadRequest);
                    }
                }

                if (rolesToAdd.Count > 0)
                {
                    var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                    if (!addResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<AdminRoleUserDetailsDto>.Fail(
                            "Failed to add roles.",
                            addResult.Errors.Select(e => e.Description).ToList(),
                            resultType: ServiceResultType.BadRequest);
                    }
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            _logger.LogInformation("Admin updated roles for user {UserId}. Roles: {Roles}", userId, string.Join(", ", normalizedRequestedRoles));

            var updatedUser = await _roleManagementRepo.GetUserAsync(userId);
            if (updatedUser == null)
                return ServiceResult<AdminRoleUserDetailsDto>.Fail("User not found after update.", resultType: ServiceResultType.NotFound);

            updatedUser.AvailableRoles = availableRoles;
            return ServiceResult<AdminRoleUserDetailsDto>.Ok(
                updatedUser,
                "User roles updated successfully.",
                code: "ZZ_ADMIN_USER_ROLES_UPDATED_SUCCESSFULLY");
        }

        private static void NormalizePaging(AdminRoleManagementQueryDto query)
        {
            if (query.PageNumber < 1)
                query.PageNumber = 1;

            if (query.PageSize < 1)
                query.PageSize = 20;

            if (query.PageSize > MaxPageSize)
                query.PageSize = MaxPageSize;
        }
    }
}
