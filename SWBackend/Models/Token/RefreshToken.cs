using SWBackend.Models.SignUp.Identity;

namespace SWBackend.Models.Token;

public class RefreshToken
{
    public int Id { get; set; }

    public AppUser? User { get; set; }
    public int UserId { get; set; }  // FK -> AspNetUsers.Id

    public string TokenHash { get; set; } = null!;
    
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public string? CreatedByIp { get; set; }
    
    // prop per la revocazione di un token durante una rotation
    public bool Revoked { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReplacedByTokenHash { get; set; }
    
    //Memorizza il browser o il dispositivo da cui il token è stato generato. Utile per sicurezza e log
    public string? UserAgent { get; set; }

    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsActive => !Revoked && !IsExpired;

}