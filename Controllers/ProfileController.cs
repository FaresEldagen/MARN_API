using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace MARN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(
            IAccountService accountService,
            ITokenService tokenService,
            ILogger<ProfileController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        //public IActionResult Index()
        //{
        //    return Ok("Profile Controller is working!");
        //}

        //public IActionResult ChangePassword()
        //{
        //    return Ok("Change Password endpoint is working!");
        //}

        //public IActionResult UpdateProfile()
        //{
        //    return Ok("Update Profile endpoint is working!");
        //}

        //public IActionResult DeleteProfile()
        //{
        //    return Ok("Delete Profile endpoint is working!");
        //}
    }
}
