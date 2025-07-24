using System.Net;
using System.Security;
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
    [Tags("AuthUser")]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailerSender _emailSender;

        public ForgotPasswordController(UserManager<AppUser> userManager, IEmailerSender emailSender)   
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Evitiamo leak di informazioni
                return Ok(new { message = "Se l'email è registrata, riceverai un link per il reset." });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);
            var callbackUrl = $"https://tuo-front-end.com/reset-password?email={request.Email}&token={encodedToken}";

            await _emailSender.SendEmailAsync(request.Email, "Resetta la tua password",
                $"Clicca <a href='{callbackUrl}'>qui</a> per reimpostare la tua password.");

            return Ok(new { message = "Email inviata se l'indirizzo esiste." });
        }
    }

