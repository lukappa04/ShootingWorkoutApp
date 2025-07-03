using Microsoft.AspNetCore.Identity;
using SWBackend.Enum;
using SWBackend.Models.Workout;

namespace SWBackend.Models.SignUp.Identity;

public class AppUser : IdentityUser<int>
{
    //Dati Anagrafici
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public int Age { get; set; }
    public DateOnly BirthDay { get; set; }

    //Ruolo Custom
    public Role RoleCode { get; set; }

    //Refresh Token
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiration { get; set; }

    //metadata utente
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdateAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeleteDate { get; set; }

    public ICollection<WorkoutM>? Workouts { get; set; }
}