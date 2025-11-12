using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using swbackend.Db.Models.SignUp.Identity;
using SWBackend.DTO.UserDto;
using SWBackend.ServiceLayer.IService.IUserService;

namespace SWBackend.Controllers.AuthUserController;

    [Route("api/[controller]")]
    [ApiController]
    [Tags("2FA")]
    public class TwoFactorAuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AppUser> _logger;

        public TwoFactorAuthenticationController(IUserService userService, ILogger<AppUser> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Verifica il codice 2FA ricevuto via mail
        /// </summary>
        /// <param name="request">Email / Code</param>
        /// <returns>Tutti i dati necessari per il login (... + JWT + RefreshToken)</returns>
        [HttpPost]
        public async Task<IActionResult> VerifyTwoFactorCode(TwoFactoryVerifyRequestDto request)
        {
            try
            {
                var response = await _userService.VerifyTwoFactorAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "2FA verification failed");
                return Unauthorized(new { message = ex.Message });
            }
        }
    }

