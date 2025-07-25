using Microsoft.AspNetCore.Identity;
using SWBackend.DTO.UserDto;

namespace SWBackend.ServiceLayer.IService.IUserService;

public interface IUserService
{
    Task<List<UserResponseDto?>> GetAllAsync();
    Task<UserResponseDto?> GetByEmailAsync(string email);
    Task<UserResponseDto?> GetByIdAsync(int id);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UsernameExistsAsync(string username);
    Task<IdentityResult> CreateUserAsync(RegisterUserRequestDto dto);
    Task<bool> CheckPasswordAsync(LoginRequestDto dto);
    Task<AuthenticationResponseDto?> LoginAsync(LoginRequestDto dto);
    Task<UserResponseDto?> GetUserByUsernameOrEmailAsync(GetUserByUsernameOrEmailRequestDto request);
    Task<UserResponseDto?> GetUserByIdAsync(GetUserByIdRequestDto request);
    Task<bool> SoftDelete(DeleteUserRequestDto request);
    Task<IdentityResult> ChangePasswordAsync(string username, ChangePasswordRequestDto request);
    Task<AuthenticationResponseDto> VerifyTwoFactorAsync(TwoFactoryVerifyRequestDto request);
    Task EnableTwoFactorAuthAsync(int userId);
    Task DisableTwoFactorAuthAsync(int userId);
    

}