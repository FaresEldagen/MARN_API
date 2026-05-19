using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Common;
using MARN_API.Enums;
using MARN_API.Enums.Account;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Hangfire;

namespace MARN_API.Services.Implementations
{
    public class AdminUserManagementService : IAdminUserManagementService
    {
        private const int MaxPageSize = 100;
        private static readonly TimeSpan ImageRestoreGracePeriod = TimeSpan.FromDays(7);
        private readonly IAdminUserManagementRepo _userManagementRepo;
        private readonly IProfileService _profileService;
        private readonly ILogger<AdminUserManagementService> _logger;

        public AdminUserManagementService(
            IAdminUserManagementRepo userManagementRepo,
            IProfileService profileService,
            ILogger<AdminUserManagementService> logger)
        {
            _userManagementRepo = userManagementRepo;
            _profileService = profileService;
            _logger = logger;
        }

        public async Task<ServiceResult<PagedResult<AdminUserListItemDto>>> GetUsersAsync(AdminUserManagementQueryDto query)
        {
            NormalizePaging(query);
            var result = await _userManagementRepo.GetUsersAsync(query);
            return ServiceResult<PagedResult<AdminUserListItemDto>>.Ok(result);
        }

        public async Task<ServiceResult<AdminUserDetailsDto>> GetUserDetailsAsync(Guid userId)
        {
            var user = await _userManagementRepo.GetUserDetailsAsync(userId);
            return user == null
                ? ServiceResult<AdminUserDetailsDto>.Fail("User not found.", resultType: ServiceResultType.NotFound)
                : ServiceResult<AdminUserDetailsDto>.Ok(user);
        }

        public async Task<ServiceResult<bool>> BanUserAsync(Guid userId)
        {
            var user = await GetManageableUserAsync(userId);
            if (!user.Success)
                return ServiceResult<bool>.Fail(user.Message!, resultType: user.ResultType);

            if (user.Data!.DeletedAt != null)
                return ServiceResult<bool>.Fail("Deleted users cannot be banned.", resultType: ServiceResultType.Conflict);

            if (user.Data.AccountStatus == AccountStatus.Banned)
                return ServiceResult<bool>.Fail("User is already banned.", resultType: ServiceResultType.Conflict);

            user.Data.StatusBeforeBan = user.Data.AccountStatus;
            user.Data.AccountStatus = AccountStatus.Banned;
            await _userManagementRepo.SaveChangesAsync();

            _logger.LogInformation("Admin banned user {UserId}", userId);
            return ServiceResult<bool>.Ok(true, "User banned successfully.");
        }

        public async Task<ServiceResult<bool>> UnbanUserAsync(Guid userId)
        {
            var user = await GetManageableUserAsync(userId);
            if (!user.Success)
                return ServiceResult<bool>.Fail(user.Message!, resultType: user.ResultType);

            if (user.Data!.DeletedAt != null)
                return ServiceResult<bool>.Fail("Deleted users cannot be unbanned.", resultType: ServiceResultType.Conflict);

            if (user.Data.AccountStatus != AccountStatus.Banned)
                return ServiceResult<bool>.Fail("Only banned users can be unbanned.", resultType: ServiceResultType.Conflict);

            user.Data.AccountStatus = user.Data.StatusBeforeBan ?? AccountStatus.Unverified;
            user.Data.StatusBeforeBan = null;
            await _userManagementRepo.SaveChangesAsync();

            _logger.LogInformation("Admin unbanned user {UserId} to status {Status}", userId, user.Data.AccountStatus);
            return ServiceResult<bool>.Ok(true, "User unbanned successfully.");
        }

        public async Task<ServiceResult<bool>> RestoreDeletedUserAsync(Guid userId)
        {
            var user = await GetManageableUserAsync(userId);
            if (!user.Success)
                return ServiceResult<bool>.Fail(user.Message!, resultType: user.ResultType);

            if (user.Data!.DeletedAt == null)
                return ServiceResult<bool>.Fail("Only deleted users can be restored.", resultType: ServiceResultType.Conflict);

            var imagesWereDeleted = user.Data.DeletedAt.Value <= DateTime.UtcNow.Subtract(ImageRestoreGracePeriod);

            if (!imagesWereDeleted && !string.IsNullOrWhiteSpace(user.Data.ImagesDeletionJob))
            {
                BackgroundJob.Delete(user.Data.ImagesDeletionJob);
            }

            user.Data.DeletedAt = null;
            user.Data.ImagesDeletionJob = null;

            if (imagesWereDeleted)
            {
                user.Data.ProfileImage = null;
                user.Data.FrontIdPhoto = null;
                user.Data.BackIdPhoto = null;

                if (user.Data.AccountStatus == AccountStatus.Verified)
                {
                    user.Data.AccountStatus = AccountStatus.Unverified;
                }

                if (user.Data.AccountStatus == AccountStatus.Banned && user.Data.StatusBeforeBan == AccountStatus.Verified)
                {
                    user.Data.StatusBeforeBan = AccountStatus.Unverified;
                }
            }

            await _userManagementRepo.SaveChangesAsync();

            _logger.LogInformation(
                "Admin restored deleted user {UserId}. Images retained: {ImagesRetained}. Current status: {Status}. Status before ban: {StatusBeforeBan}",
                userId,
                !imagesWereDeleted,
                user.Data.AccountStatus,
                user.Data.StatusBeforeBan);

            return ServiceResult<bool>.Ok(true, "User restored successfully.");
        }

        public async Task<ServiceResult<bool>> DeleteUserAsync(Guid userId)
        {
            var user = await GetManageableUserAsync(userId);
            if (!user.Success)
                return ServiceResult<bool>.Fail(user.Message!, resultType: user.ResultType);

            if (user.Data!.DeletedAt != null)
                return ServiceResult<bool>.Fail("User is already deleted.", resultType: ServiceResultType.Conflict);

            _logger.LogInformation("Admin requested soft delete for user {UserId}", userId);
            return await _profileService.DeleteUserAsync(userId);
        }

        private async Task<ServiceResult<ApplicationUser>> GetManageableUserAsync(Guid userId)
        {
            if (await _userManagementRepo.IsAdminUserAsync(userId))
                return ServiceResult<ApplicationUser>.Fail("Admin users cannot be managed from this endpoint.", resultType: ServiceResultType.Forbidden);

            var user = await _userManagementRepo.GetUserByIdAsync(userId);
            return user == null
                ? ServiceResult<ApplicationUser>.Fail("User not found.", resultType: ServiceResultType.NotFound)
                : ServiceResult<ApplicationUser>.Ok(user);
        }

        private static void NormalizePaging(AdminUserManagementQueryDto query)
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
