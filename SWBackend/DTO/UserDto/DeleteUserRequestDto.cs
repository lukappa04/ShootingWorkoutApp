namespace SWBackend.DTO.UserDto;

public sealed class DeleteUserRequestDto
{
    public int UserIdD { get; set; }
    public string UsernameOrEmailD { get; set; } = string.Empty;
}