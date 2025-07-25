using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using SWBackend.DTO.UserDto;
using SWBackend.Models.SignUp.Identity;
using SWBackend.ServiceLayer.IService.IUserService;
using SWBackend.ServiceLayer.Mail;

namespace SWBackend.Controllers.AuthUserController;

    [Route("api/[controller]")]
    [ApiController]
    [Tags("AuthUser/PasswordReset")]
    public class ResetPasswordController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailerSender _emailSender;

        public ResetPasswordController(UserManager<AppUser> userManager, IEmailerSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }
        /// <summary>
        /// Metodo che si pospone all ForgotPassword, una volta preso il token si inserisci nel body, insieme alla mail da cui è stato preso.
        /// Se le due cose combaciano si può procedere e controllare che le due password (nuove) inserite combacino.
        /// Se è così return 200(message)
        /// </summary>
        /// <param name="request">DTO: Email / Token / NewPassword / ConfirmPassword</param>
        /// <returns>200</returns>

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
        {
            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest(new { message = "Le password non coincidono." });
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Utente non trovato." });
            }

            var result = await _userManager.ResetPasswordAsync(user, WebUtility.UrlDecode(request.Token), request.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "Password reimpostata con successo." });
        }
    }

