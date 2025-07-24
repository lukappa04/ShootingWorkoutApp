using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SWBackend.Models.SignUp.Identity;

namespace SWBackend.Controllers.AuthUserController;

[Route("api/[controller]")]
[ApiController]
public class ConfirmEmailController : ControllerBase
{
    private readonly ILogger<ConfirmEmailController> _logger;
    private readonly UserManager<AppUser> _userManager;

    public ConfirmEmailController(ILogger<ConfirmEmailController> logger, UserManager<AppUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return BadRequest("Utente non trovato");

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
            return Ok("Email confermata con successo!");
        
        return BadRequest("Errore nella conferma dell'email.");
    }
}

