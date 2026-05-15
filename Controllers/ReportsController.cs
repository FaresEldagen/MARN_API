using MARN_API.DTOs.Common;
using MARN_API.DTOs.Moderation;
using MARN_API.Models;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MARN_API.Controllers
{
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

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseDto<ReportSubmissionResultDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SubmitReport([FromBody] SubmitReportDto request)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(CreateErrorResponse(StatusCodes.Status401Unauthorized, "User ID not found in token"));

            var result = await _reportService.SubmitReportAsync(userId, request);
            return HandleServiceResult(result);
        }
    }
}
