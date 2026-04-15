using MARN_API.DTOs.Dashboard;
using MARN_API.DTOs.Profile;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IProfileService
    {
        #region Profile and Dashboards
        public Task<ServiceResult<RenterDashboardDto>> RenterDashboardAsync(Guid userId);
        public Task<ServiceResult<OwnerDashboardDto>> OwnerDashboardAsync(Guid userId);
        public Task<ServiceResult<ProfileDto>> GetProfileAsync(Guid userId);
        #endregion


        #region Profile Settings
        public Task<ServiceResult<ProfileSettingsDto>> GetProfileSettingsAsync(Guid userId);
        public Task<ServiceResult<bool>> UpdateProfileBasicDataAsync(UpdateProfileDto updateProfileDto);
        public Task<ServiceResult<bool>> UpdateProfileLegalDataAsync(UpdateLegalDto updateLegalDto);
        public Task<ServiceResult<bool>> UpdateProfileRoommatePreferencesDataAsync(UpdateRoommatePreferencesDto updateRoommatePreferencesDto);
        public Task<ServiceResult<bool>> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        public Task<ServiceResult<bool>> ToggleTwoFactorAsync(Guid userId, string? password = null);
        public Task<ServiceResult<bool>> DeleteUserAsync(Guid userId);
        #endregion
    }
}
