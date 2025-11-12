using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using swbackend.Db.Models.SignUp.Identity;
using SWBackend.ServiceLayer.IService.IUserService;

namespace SWBackend.Controllers.AuthUserController;

    [Route("api/[controller]")]
    [ApiController]
    [Tags("2FA")]
    public class EnableTwoFactorAuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AppUser> _logger;

        public EnableTwoFactorAuthController(IUserService userService, ILogger<AppUser> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Attiva la 2FA dal login successivo a quello effettuato
        /// Step: Login > Enable2FA > logout > Login > Check the Email +  Copy the code > TwoFactorAuthentication.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Enable2FA()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null)
                    return Unauthorized();

                var userId = int.Parse(userIdClaim);
                await _userService.EnableTwoFactorAuthAsync(userId);
                return Ok(new { message = "2FA enabled" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errored enabling twofactor");
                return BadRequest(new { message = ex.Message });
            }
        }
        
    }

