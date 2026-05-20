using MARN_API.DTOs.Common;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MARN_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : BaseController
    {
        private readonly IContactSupportService _contactSupportService;

        public SupportController(IContactSupportService contactSupportService)
        {
            _contactSupportService = contactSupportService;
        }

        /// <summary>
        /// Sends a contact-us message to the configured support email.
        /// </summary>
        /// <param name="request">The user's contact details and message.</param>
        /// <response code="200">The support message was sent successfully.</response>
        /// <response code="400">The request is invalid or support email sending failed.</response>
        [AllowAnonymous]
        [HttpPost("contact-us")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ContactUs([FromBody] ContactSupportRequestDto request)
        {
            TryGetUserId(out var userId);

            var result = await _contactSupportService.SendContactUsEmailAsync(request,userId);
            return HandleServiceResult(result);
        }
    }
}
