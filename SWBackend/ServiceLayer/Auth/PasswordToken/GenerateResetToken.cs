using Microsoft.AspNetCore.Identity;
using SWBackend.Models.SignUp.Identity;

namespace SWBackend.ServiceLayer.Auth.PasswordToken;

public class GenerateResetToken
{
    private readonly UserManager<AppUser> _userManager;

    public GenerateResetToken(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<string> GenerateResetPasswordTokenAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new Exception("Utente non trovato");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return token; // da inviare per email
    }
}
