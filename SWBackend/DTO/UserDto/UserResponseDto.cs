
namespace SWBackend.DTO.UserDto;

public sealed class UserResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public int Age { get; set; }
    public DateOnly BirthDay { get; set; }
    public string Role { get; set; } = string.Empty;

}