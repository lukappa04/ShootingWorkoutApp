using SWBackend.Enum;

namespace SWBackend.DTO.UserDto;

public sealed class RegisterUserRequestDto
{
    public string NameD { get; set; } = string.Empty;
    public string SurnameD { get; set; } = string.Empty;
    public int AgeD { get; set; }
    public string UsernameD { get; set; } = string.Empty;
    public string EmailD { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateOnly BirthDay { get; set; }
    public DateTime CreatedAtD { get; set; }
}