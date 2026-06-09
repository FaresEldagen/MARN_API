using MARN_API.DTOs.Common;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MARN_API.Attributes;

namespace MARN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [CheckRole("Admin")]
    public class CsvSeedController : BaseController
    {
        private readonly ICsvSeedImportService _csvSeedImportService;

        public CsvSeedController(ICsvSeedImportService csvSeedImportService)
        {
            _csvSeedImportService = csvSeedImportService;
        }

        [HttpPost("properties")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ImportProperties([FromForm] SeedCsvUploadDto dto)
        {
            var result = await _csvSeedImportService.ImportPropertiesAsync(dto.File);
            return HandleServiceResult(result);
        }

        [HttpPost("users")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ImportUsers([FromForm] SeedCsvUploadDto dto)
        {
            var result = await _csvSeedImportService.ImportUsersAsync(dto.File);
            return HandleServiceResult(result);
        }

        [HttpPost("user-activities")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ImportUserActivities([FromForm] SeedCsvUploadDto dto)
        {
            var result = await _csvSeedImportService.ImportUserActivitiesAsync(dto.File);
            return HandleServiceResult(result);
        }
    }
}
