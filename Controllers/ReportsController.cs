using MARN_API.DTOs.Common;
using MARN_API.DTOs.Moderation;
using MARN_API.Models;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MARN_API.Controllers
{
    /// <summary>
    /// Allows authenticated users to submit moderation reports against supported targets.
    /// </summary>
    /// <remarks>
    /// This controller is part of the moderation-reporting stage. Reports are later reviewed through
    /// the admin moderation endpoints in <c>AdminController</c>.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : BaseController
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Submits a moderation report against a user, property, message, or property comment.
        /// </summary>
        /// <param name="request">
        /// The report payload containing the target type, the target identifier as a string,
        /// and the human-written reason for the report.
        /// </param>
        /// <remarks>
        /// <para>
        /// <c>ReportableTargetId</c> is parsed based on <c>ReportableType</c>:
        /// user and message reports require a GUID; property and property-comment reports require
        /// a positive numeric identifier.
        /// </para>
        /// <para>
        /// Message reports are limited to participants in that conversation, and users cannot report
        /// their own accounts.
        /// </para>
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseDto<ReportSubmissionResultDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SubmitReport([FromBody] SubmitReportDto request)
        {
            if (!TryGetUserId(out var userId))
                return UnauthorizedUserIdMissing();

            var result = await _reportService.SubmitReportAsync(userId, request);
            return HandleServiceResult(result);
        }
    }
}
