namespace SWBackend.DTO.UserDto;

public sealed class UpdateCredentialRequestDto
{
    public string UsernameD { get; set; } = string.Empty;
    public string EmailD { get; set; } = string.Empty;
    public string PasswordD { get; set; } = string.Empty;
}