using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWBackend.Models.SignUp.Identity;
using SWBackend.ServiceLayer.IService.IUserService;

namespace SWBackend.Controllers.AuthUserController;

    [Route("api/[controller]")]
    [ApiController]
    [Tags("2FA")]
    public class DisableTwoFactorAuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AppUser> _logger;

        public DisableTwoFactorAuthController(IUserService userService, ILogger<AppUser> logger)            
        {
            _userService = userService;
            _logger = logger;
        }
    /// <summary>
    /// Disbilità la 2FA
    /// </summary>
    /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DisableTwoFactorAuth()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim);

            await _userService.DisableTwoFactorAuthAsync(userId);
            return Ok(new { message = "2FA disattivato correttamente" });
        }
    }

