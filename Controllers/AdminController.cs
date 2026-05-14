using MARN_API.DTOs.Admin;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MARN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly IAdminDashboardService _adminDashboardService;
        private readonly IAdminVerificationService _adminVerificationService;
        private readonly IAdminUserManagementService _adminUserManagementService;
        private readonly IAdminRoleManagementService _adminRoleManagementService;

        public AdminController(
            IAdminDashboardService adminDashboardService,
            IAdminVerificationService adminVerificationService,
            IAdminUserManagementService adminUserManagementService,
            IAdminRoleManagementService adminRoleManagementService)
        {
            _adminDashboardService = adminDashboardService;
            _adminVerificationService = adminVerificationService;
            _adminUserManagementService = adminUserManagementService;
            _adminRoleManagementService = adminRoleManagementService;
        }

        [HttpGet("dashboard/overview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetDashboardOverview()
        {
            var result = await _adminDashboardService.GetOverviewAsync();
            return HandleServiceResult<AdminDashboardOverviewDto>(result);
        }

        [HttpGet("verifications/users/pending")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPendingUserVerifications([FromQuery] AdminVerificationQueryDto query)
        {
            var result = await _adminVerificationService.GetPendingUserVerificationsAsync(query);
            return HandleServiceResult(result);
        }

        [HttpGet("verifications/users/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserVerificationDetails(Guid userId)
        {
            var result = await _adminVerificationService.GetUserVerificationDetailsAsync(userId);
            return HandleServiceResult(result);
        }

        [HttpPatch("verifications/users/{userId:guid}/approve")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ApproveUserVerification(Guid userId)
        {
            var result = await _adminVerificationService.ApproveUserVerificationAsync(userId);
            return HandleServiceResult(result);
        }

        [HttpPatch("verifications/users/{userId:guid}/decline")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeclineUserVerification(Guid userId, [FromBody] AdminVerificationDecisionDto decision)
        {
            var result = await _adminVerificationService.DeclineUserVerificationAsync(userId, decision);
            return HandleServiceResult(result);
        }

        [HttpGet("verifications/properties/pending")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPendingPropertyVerifications([FromQuery] AdminVerificationQueryDto query)
        {
            var result = await _adminVerificationService.GetPendingPropertyVerificationsAsync(query);
            return HandleServiceResult(result);
        }

        [HttpGet("verifications/properties/{propertyId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPropertyVerificationDetails(long propertyId)
        {
            var result = await _adminVerificationService.GetPropertyVerificationDetailsAsync(propertyId);
            return HandleServiceResult(result);
        }

        [HttpPatch("verifications/properties/{propertyId:long}/approve")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ApprovePropertyVerification(long propertyId)
        {
            var result = await _adminVerificationService.ApprovePropertyVerificationAsync(propertyId);
            return HandleServiceResult(result);
        }

        [HttpPatch("verifications/properties/{propertyId:long}/decline")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeclinePropertyVerification(long propertyId, [FromBody] AdminVerificationDecisionDto decision)
        {
            var result = await _adminVerificationService.DeclinePropertyVerificationAsync(propertyId, decision);
            return HandleServiceResult(result);
        }

        [HttpGet("users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUsers([FromQuery] AdminUserManagementQueryDto query)
        {
            var result = await _adminUserManagementService.GetUsersAsync(query);
            return HandleServiceResult(result);
        }

        [HttpGet("users/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserDetails(Guid userId)
        {
            var result = await _adminUserManagementService.GetUserDetailsAsync(userId);
            return HandleServiceResult(result);
        }

        [HttpPatch("users/{userId:guid}/ban")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> BanUser(Guid userId)
        {
            var result = await _adminUserManagementService.BanUserAsync(userId);
            return HandleServiceResult(result);
        }

        [HttpPatch("users/{userId:guid}/restore")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RestoreUser(Guid userId)
        {
            var result = await _adminUserManagementService.RestoreUserAsync(userId);
            return HandleServiceResult(result);
        }

        [HttpDelete("users/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var result = await _adminUserManagementService.DeleteUserAsync(userId);
            return HandleServiceResult(result);
        }

        [HttpGet("roles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _adminRoleManagementService.GetRolesAsync();
            return HandleServiceResult(result);
        }

        [HttpGet("roles/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUsersForRoleManagement([FromQuery] AdminRoleManagementQueryDto query)
        {
            var result = await _adminRoleManagementService.GetUsersAsync(query);
            return HandleServiceResult(result);
        }

        [HttpGet("roles/users/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoleManagementUser(Guid userId)
        {
            var result = await _adminRoleManagementService.GetUserAsync(userId);
            return HandleServiceResult(result);
        }

        [HttpPatch("roles/users/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateUserRoles(Guid userId, [FromBody] AdminUpdateUserRolesDto request)
        {
            var result = await _adminRoleManagementService.UpdateUserRolesAsync(userId, request);
            return HandleServiceResult(result);
        }
    }
}
