using swbackend.Db.Enum;

namespace SWBackend.DTO.UserDto;
/// <summary>
/// DTO: contiene tutti i dati utili per l'autenticazione dell'utente
/// </summary>
public sealed class AuthenticationResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
    public string Token { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
    public bool Requires2FA { get; set; }
}