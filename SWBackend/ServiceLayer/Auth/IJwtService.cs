using swbackend.Db.Models.SignUp.Identity;
using SWBackend.Models.SignUp.Identity;

namespace SWBackend.ServiceLayer.Auth;

public interface IJwtService
{
    /// <summary>
    /// Crea un JWT firmato con chiave SHA256, contente le informazioni principali del utente
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    string GenerateToken(AppUser? user);
    string GenerateRefreshToken();
}