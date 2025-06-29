using SWBackend.Models.SignUp.Identity;

namespace SWBackend.ServiceLayer.Auth;

public interface IJwtService
{
    string GenerateToken(AppUser user);
}