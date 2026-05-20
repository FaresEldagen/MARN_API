using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Moderation;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MARN_API.Controllers
{
    /// <summary>
    /// Provides the admin-dashboard API surface for overview metrics, moderation,
    /// verifications, role management, operational actions, and analytics exports.
    /// </summary>
    /// <remarks>
    /// All endpoints in this controller require an authenticated user in the <c>Admin</c> role.
    /// Success responses are wrapped in <c>ApiResponseDto&lt;T&gt;</c>. Validation, conflict,
    /// not-found, forbidden, and unauthorized cases return the shared <c>ErrorResponse</c> shape.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly IAdminDashboardService _adminDashboardService;
        private readonly IAdminAnalyticsReportService _adminAnalyticsReportService;
        private readonly IAdminDetailedStatsService _adminDetailedStatsService;
        private readonly IAdminVerificationService _adminVerificationService;
        private readonly IAdminUserManagementService _adminUserManagementService;
        private readonly IAdminRoleManagementService _adminRoleManagementService;
        private readonly IAdminReportModerationService _adminReportModerationService;
        private readonly IContractService _contractService;

        public AdminController(
            IAdminDashboardService adminDashboardService,
            IAdminAnalyticsReportService adminAnalyticsReportService,
            IAdminDetailedStatsService adminDetailedStatsService,
            IAdminVerificationService adminVerificationService,
            IAdminUserManagementService adminUserManagementService,
            IAdminRoleManagementService adminRoleManagementService,
            IAdminReportModerationService adminReportModerationService,
            IContractService contractService)
        {
            _adminDashboardService = adminDashboardService;
            _adminAnalyticsReportService = adminAnalyticsReportService;
            _adminDetailedStatsService = adminDetailedStatsService;
            _adminVerificationService = adminVerificationService;
            _adminUserManagementService = adminUserManagementService;
            _adminRoleManagementService = adminRoleManagementService;
            _adminReportModerationService = adminReportModerationService;
            _contractService = contractService;
        }

        /// <summary>
        /// Returns the top-level admin dashboard snapshot used by the landing page cards and chart.
        /// </summary>
        /// <remarks>
        /// The response includes total users, properties, pending verifications, contract totals,
        /// sales, revenue, new users this month, active contracts, and the six most recent monthly
        /// revenue points used by the overview graph.
        /// </remarks>
        [HttpGet("dashboard/overview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetDashboardOverview()
        {
            var result = await _adminDashboardService.GetOverviewAsync();
            return HandleServiceResult<AdminDashboardOverviewDto>(result);
        }

        /// <summary>
        /// Generates and stores an admin analytics report file.
        /// </summary>
        /// <param name="request">
        /// The export request containing report scope, output format, requested period,
        /// and optional custom date range when <c>Period</c> is <c>Custom</c>.
        /// </param>
        /// <remarks>
        /// <para>
        /// Supported scopes are <c>Overview</c>, <c>Users</c>, <c>Properties</c>,
        /// <c>Contracts</c>, <c>Revenue</c>, and <c>Full</c>.
        /// </para>
        /// <para>
        /// <c>Full</c> exports are only available as PDF. When <c>Period</c> is <c>Custom</c>,
        /// both <c>FromUtc</c> and <c>ToUtc</c> are required and <c>FromUtc</c> must be earlier
        /// than <c>ToUtc</c>.
        /// </para>
        /// </remarks>
        [HttpPost("analytics-reports/generate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GenerateAnalyticsReport([FromBody] AdminAnalyticsReportGenerateRequestDto request)
        {
            if (!TryGetUserId(out var adminId))
                return UnauthorizedUserIdMissing();

            var result = await _adminAnalyticsReportService.GenerateAsync(adminId, request);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Returns paged analytics report history for previously generated admin exports.
        /// </summary>
        /// <param name="query">
        /// Optional filters by scope, format, year, month, and page settings.
        /// </param>
        /// <remarks>
        /// <para>
        /// <c>Year</c> and <c>Month</c> filter against <c>FromUtc</c> when the record has a stored
        /// period start; otherwise they fall back to the report generation timestamp.
        /// </para>
        /// <para>
        /// Page numbers lower than 1 are normalized to 1. Page sizes lower than 1 are normalized
        /// to 20. Page sizes above 100 are clamped to 100.
        /// </para>
        /// </remarks>
        [HttpGet("analytics-reports")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAnalyticsReports([FromQuery] AdminAnalyticsReportQueryDto query)
        {
            var result = await _adminAnalyticsReportService.GetReportsAsync(query);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Returns metadata for a single generated analytics report.
        /// </summary>
        /// <param name="reportId">The analytics report identifier.</param>
        [HttpGet("analytics-reports/{reportId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAnalyticsReport(long reportId)
        {
            var result = await _adminAnalyticsReportService.GetReportAsync(reportId);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Downloads a previously generated analytics report file.
        /// </summary>
        /// <param name="reportId">The analytics report identifier.</param>
        /// <remarks>
        /// Returns the stored PDF or CSV file bytes. A 404 is returned if the database record
        /// exists but the physical file is missing from disk.
        /// </remarks>
        [HttpGet("analytics-reports/{reportId:long}/download")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadAnalyticsReport(long reportId)
        {
            var result = await _adminAnalyticsReportService.DownloadAsync(reportId);
            if (!result.Success)
                return HandleServiceResult(result);

            return File(result.Data!.FileBytes, result.Data.ContentType, result.Data.FileName);
        }

        /// <summary>
        /// Returns detailed user statistics and a paged user table for the selected period.
        /// </summary>
        /// <param name="query">
        /// Period, paging, and optional filters for search text, account status, role, and deleted inclusion.
        /// </param>
        /// <remarks>
        /// Search matches the user's full name and email address. The result includes summary counts,
        /// status breakdowns, role breakdowns, a time series of user creation counts, and a paged table.
        /// In this stage, <c>Period</c> is a string and supports <c>allTime</c>, <c>thisMonth</c>,
        /// <c>thisYear</c>, and <c>custom</c>.
        /// </remarks>
        [HttpGet("stats/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetDetailedUsersStats([FromQuery] AdminDetailedUsersQueryDto query)
        {
            var result = await _adminDetailedStatsService.GetUsersAsync(query);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Returns detailed property statistics and a paged property table for the selected period.
        /// </summary>
        /// <param name="query">
        /// Period, paging, and optional filters for search text, verification status, type,
        /// governorate, active flag, and deleted inclusion.
        /// </param>
        /// <remarks>
        /// Search matches property title, address, and owner full name. Governorate filtering compares
        /// the request value to the stored <c>State</c> field case-insensitively.
        /// </remarks>
        [HttpGet("stats/properties")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetDetailedPropertiesStats([FromQuery] AdminDetailedPropertiesQueryDto query)
        {
            var result = await _adminDetailedStatsService.GetPropertiesAsync(query);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Returns a deep admin view of a single property, including owner info, media, amenities,
        /// comments, ratings, contracts, and booking requests.
        /// </summary>
        /// <param name="propertyId">The property identifier.</param>
        [HttpGet("stats/properties/{propertyId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetailedProperty(long propertyId)
        {
            var result = await _adminDetailedStatsService.GetPropertyDetailsAsync(propertyId);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Soft deletes a property from the detailed properties admin area.
        /// </summary>
        /// <param name="propertyId">The property identifier.</param>
        /// <remarks>
        /// This reuses the existing property soft-delete workflow, including the delayed Hangfire image
        /// cleanup job that runs after seven days.
        /// </remarks>
        [HttpDelete("stats/properties/{propertyId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteDetailedProperty(long propertyId)
        {
            var result = await _adminDetailedStatsService.DeletePropertyAsync(propertyId);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Restores a soft-deleted property from the detailed properties admin area.
        /// </summary>
        /// <param name="propertyId">The property identifier.</param>
        /// <remarks>
        /// If the delayed Hangfire image cleanup job is still pending, it is cancelled. If the seven-day
        /// grace window has already elapsed and the proof-of-ownership image was removed, the property is
        /// restored in <c>Pending</c> status so verification can be resubmitted.
        /// </remarks>
        [HttpPatch("stats/properties/{propertyId:long}/restore-deleted")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RestoreDeletedDetailedProperty(long propertyId)
        {
            var result = await _adminDetailedStatsService.RestoreDeletedPropertyAsync(propertyId);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Returns detailed contract statistics and a paged contract table for the selected period.
        /// </summary>
        /// <param name="query">
        /// Period, paging, and optional filters for search text and contract status.
        /// </param>
        /// <remarks>
        /// Search matches the contract ID, property title, owner full name, and renter full name.
        /// The response includes total contract value, status breakdowns, and per-row flags such as <c>CanCancel</c>.
        /// </remarks>
        [HttpGet("stats/contracts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetDetailedContractsStats([FromQuery] AdminDetailedContractsQueryDto query)
        {
            var result = await _adminDetailedStatsService.GetContractsAsync(query);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Downloads the contract PDF for a contract directly from the admin detailed-contracts area.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <remarks>
        /// This endpoint reuses the core contract download flow. Admins can download by contract ID
        /// without being the owner or renter.
        /// </remarks>
        [HttpGet("stats/contracts/{contractId:long}/download")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadDetailedContract(long contractId)
        {
            if (!TryGetUserId(out var adminId))
                return UnauthorizedUserIdMissing();

            var result = await _contractService.DownloadContractAsync(adminId, contractId);
            if (!result.Success)
                return HandleServiceResult(result);

            return File(result.Data!.FileBytes, result.Data.ContentType, result.Data.FileName);
        }

        /// <summary>
        /// Cancels an active or pending contract from the detailed-contracts admin area.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <remarks>
        /// The action is one-way. It cancels eligible contracts and attempts to cancel any issued
        /// unpaid Stripe payment intents before the contract status is changed.
        /// </remarks>
        [HttpPatch("stats/contracts/{contractId:long}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CancelDetailedContract(long contractId)
        {
            var result = await _adminDetailedStatsService.CancelContractAsync(contractId);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Returns detailed payment and revenue statistics for the selected period.
        /// </summary>
        /// <param name="query">
        /// Period, paging, and optional filters for search text and payment status.
        /// </param>
        /// <remarks>
        /// Search matches payment ID, contract ID, property title, owner full name, and renter full name.
        /// The response includes totals for payments, sales, platform revenue, owner payouts, a revenue time
        /// series, and a paged payment table.
        /// </remarks>
        [HttpGet("stats/revenue")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetDetailedRevenueStats([FromQuery] AdminDetailedRevenueQueryDto query)
        {
            var result = await _adminDetailedStatsService.GetRevenueAsync(query);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Returns the paged queue of users whose legal identity documents are awaiting admin review.
        /// </summary>
        /// <param name="query">Paging input with optional page number and page size.</param>
        [HttpGet("verifications/users/pending")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPendingUserVerifications([FromQuery] AdminVerificationQueryDto query)
        {
            var result = await _adminVerificationService.GetPendingUserVerificationsAsync(query);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Returns the full verification payload for a single user review request.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
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

        /// <summary>
        /// Approves a pending user verification request.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <remarks>
        /// Only users currently in <c>Pending</c> account status can be approved. On success,
        /// the account status becomes <c>Verified</c>.
        /// </remarks>
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

        /// <summary>
        /// Declines a pending user verification request.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="decision">
        /// Optional review payload containing a free-text reason. The reason is accepted and logged
        /// but is not persisted in a dedicated audit table in the current implementation.
        /// </param>
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

        /// <summary>
        /// Returns the paged queue of properties whose ownership documents are awaiting admin review.
        /// </summary>
        /// <param name="query">Paging input with optional page number and page size.</param>
        [HttpGet("verifications/properties/pending")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPendingPropertyVerifications([FromQuery] AdminVerificationQueryDto query)
        {
            var result = await _adminVerificationService.GetPendingPropertyVerificationsAsync(query);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Returns the full verification payload for a single property review request.
        /// </summary>
        /// <param name="propertyId">The property identifier.</param>
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

        /// <summary>
        /// Approves a pending property verification request.
        /// </summary>
        /// <param name="propertyId">The property identifier.</param>
        /// <remarks>
        /// Only properties currently in <c>Pending</c> verification status can be approved.
        /// On success, the property status becomes <c>Verified</c>.
        /// </remarks>
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

        /// <summary>
        /// Declines a pending property verification request.
        /// </summary>
        /// <param name="propertyId">The property identifier.</param>
        /// <param name="decision">
        /// Optional review payload containing a free-text reason. The reason is accepted and logged
        /// but is not persisted in a dedicated audit table in the current implementation.
        /// </param>
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

        /// <summary>
        /// Returns the paged admin user-management table.
        /// </summary>
        /// <param name="query">
        /// Optional filters for search text, account status, role, deleted inclusion, and paging.
        /// </param>
        /// <remarks>
        /// Search matches full name, email, and phone number. Admin-role users are excluded from this
        /// endpoint by design; they are managed through the role-management stage instead.
        /// </remarks>
        [HttpGet("users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUsers([FromQuery] AdminUserManagementQueryDto query)
        {
            var result = await _adminUserManagementService.GetUsersAsync(query);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Returns a full admin view of one manageable user, including roles, properties,
        /// contracts, and payment summaries.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
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

        /// <summary>
        /// Bans a manageable non-admin user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <remarks>
        /// Deleted users cannot be banned. The current account status is preserved in
        /// <c>StatusBeforeBan</c> so that restore can return the user to the correct prior state.
        /// </remarks>
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

        /// <summary>
        /// Removes a ban from a manageable non-admin user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <remarks>
        /// Only users currently in <c>Banned</c> status can be unbanned. Deleted users cannot be unbanned
        /// through this endpoint. The restored state comes from <c>StatusBeforeBan</c>, not a forced
        /// <c>Verified</c> fallback.
        /// </remarks>
        [HttpPatch("users/{userId:guid}/unban")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UnbanUser(Guid userId)
        {
            var result = await _adminUserManagementService.UnbanUserAsync(userId);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Restores a soft-deleted manageable non-admin user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <remarks>
        /// This endpoint reverses the user soft-delete. If the delayed Hangfire image cleanup job is still
        /// pending, it is cancelled so the stored media stays available. If the seven-day grace window has
        /// already elapsed and the user's verification images were removed, the account is restored in an
        /// unverified state so identity verification can be resubmitted.
        /// </remarks>
        [HttpPatch("users/{userId:guid}/restore")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RestoreDeletedUser(Guid userId)
        {
            var result = await _adminUserManagementService.RestoreDeletedUserAsync(userId);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Soft deletes a manageable non-admin user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
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

        /// <summary>
        /// Returns all identity roles known to the system with assignment metadata.
        /// </summary>
        /// <remarks>
        /// Base roles such as <c>Owner</c> and <c>Renter</c> are marked as protected and not assignable.
        /// Future roles such as <c>Moderator</c> appear automatically because the list is read dynamically
        /// from identity roles.
        /// </remarks>
        [HttpGet("roles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _adminRoleManagementService.GetRolesAsync();
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Returns the paged role-management user table.
        /// </summary>
        /// <param name="query">
        /// Optional filters for search text, account status, role, deleted inclusion, and paging.
        /// </param>
        /// <remarks>
        /// Search matches full name, email, and user name. Unlike the user-management stage,
        /// admin users are included here because admins themselves can be assigned or removed.
        /// </remarks>
        [HttpGet("roles/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUsersForRoleManagement([FromQuery] AdminRoleManagementQueryDto query)
        {
            var result = await _adminRoleManagementService.GetUsersAsync(query);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Returns a single user's role-management details, including available roles.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
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

        /// <summary>
        /// Replaces the user's assignable roles while preserving protected base roles.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="request">
        /// The role-update payload. <c>Roles</c> may contain assignable roles such as <c>Admin</c>
        /// or <c>Moderator</c>. <c>Owner</c> and <c>Renter</c> are preserved automatically.
        /// </param>
        /// <remarks>
        /// Duplicate role names are ignored. Unknown roles and protected roles in the payload
        /// return 400. Removing the last remaining admin returns 409.
        /// </remarks>
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

        /// <summary>
        /// Returns the moderation queue with paging, breakdowns, and optional filters.
        /// </summary>
        /// <param name="query">
        /// Optional filters for report status, reportable type, free-text search, and paging.
        /// </param>
        /// <remarks>
        /// Search matches report reason, reporter, reviewer, target IDs, and target-specific text
        /// such as user names, property titles, message participants, or property-comment content.
        /// </remarks>
        [HttpGet("reports")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetReports([FromQuery] AdminReportQueryDto query)
        {
            var result = await _adminReportModerationService.GetReportsAsync(query);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Returns the full details of a single moderation report.
        /// </summary>
        /// <param name="reportId">The report identifier.</param>
        [HttpGet("reports/{reportId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReportDetails(long reportId)
        {
            var result = await _adminReportModerationService.GetReportDetailsAsync(reportId);
            return HandleServiceResult(result);
        }

        /// <summary>
        /// Reviews a moderation report and optionally applies one or more compatible actions.
        /// </summary>
        /// <param name="reportId">The report identifier.</param>
        /// <param name="request">
        /// Review payload containing the final report status, optional note, and optional list of actions.
        /// </param>
        /// <remarks>
        /// Rejected reports cannot apply actions. Supported actions depend on the reported target type:
        /// user reports allow user bans, property reports allow property deactivation and owner bans,
        /// message reports allow message hiding and sender bans, and property-comment reports allow
        /// comment hiding and commenter bans.
        /// </remarks>
        [HttpPatch("reports/{reportId:long}/review")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ReviewReport(long reportId, [FromBody] AdminReviewReportDto request)
        {
            if (!TryGetUserId(out var adminId))
                return UnauthorizedUserIdMissing();

            var result = await _adminReportModerationService.ReviewReportAsync(adminId, reportId, request);
            return HandleServiceResult(result);
        }
    }
}
