using Microsoft.AspNetCore.Identity;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using SWBackend.DTO.UserDto;
using SWBackend.Models.SignUp.Identity;


namespace SWBackend.RepositoryLayer.IRepository.User;

public interface IUserRepository
{
    Task<List<AppUser>> GetAllUserAsync();
    Task<AppUser?> GetByEmailAsync(string email);
    Task<AppUser?> GetByIdAsync(int id);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UsernameExistsAsync(string username);
    Task<IdentityResult> CreateUserAsync(AppUser user, string password);
    Task<bool> CheckPasswordAsync(AppUser user, string password);

}