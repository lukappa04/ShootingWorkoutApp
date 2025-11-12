using System.Net;
using System.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using swbackend.Db.Models.SignUp.Identity;
using SWBackend.DTO.UserDto;
using SWBackend.ServiceLayer.Auth.Mail;

namespace SWBackend.Controllers.AuthUserController;

    [Route("api/[controller]")]
    [ApiController]
    [Tags("AuthUser/PasswordReset")]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailerSender _emailSender;

        public ForgotPasswordController(UserManager<AppUser> userManager, IEmailerSender emailSender)   
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Metodo post per l'invio della mail alla mail dell'utente specificata. Viene effettuato un controllo a db che la mail esista e che sia stata attivata.
        /// Se passa i controlli verrà inviata con su email di riferimento ed un suo token che poi successivamente da frontend verrà preso ed usato per il cambio password
        /// </summary>
        /// <param name="request">DTO: Email</param>
        /// <returns>200 se tutto va bene / 200 anche se non viene trovato, per non dare indizi. cambia solo il messaggio che verrà visualizzato</returns>
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

