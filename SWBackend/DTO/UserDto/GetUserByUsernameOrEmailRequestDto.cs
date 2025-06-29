namespace SWBackend.DTO.UserDto;

public sealed class GetUserByUsernameOrEmailRequestDto
{
    public string UsernameOrEmailD { get; set; } = string.Empty;
}