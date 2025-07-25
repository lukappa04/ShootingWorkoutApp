namespace SWBackend.DTO.UserDto;

public class TwoFactoryVerifyRequestDto
{
    public string Email { get; set; } = String.Empty;
    public string Code { get; set; } = String.Empty;
}