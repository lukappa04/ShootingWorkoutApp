namespace SWBackend.DTO.UserDto;

public sealed class LoginRequestDto
{
    public string UsernameOrEmailD { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}