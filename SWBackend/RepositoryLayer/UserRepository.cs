using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SWBackend.DataBase;
using SWBackend.DTO.UserDto;
using SWBackend.Models.SignUp.Identity;
using SWBackend.RepositoryLayer.IRepository.User;

namespace SWBackend.RepositoryLayer;

public class UserRepository : IUserRepository
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SWDbContext _context;

    public UserRepository(UserManager<AppUser> userManager, SWDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<bool> CheckPasswordAsync(AppUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _userManager.Users.AnyAsync(u => u.Email == email);
        
    }

    public async Task<List<AppUser>> GetAllUserAsync()
    {
        return await _context.Users
        .Where(u => u.DeleteDate == null)
        .ToListAsync();
    }

    public async Task<AppUser?> GetByEmailAsync(string email)
    {
        return await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<AppUser?> GetByIdAsync(int id)
    {
        return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _userManager.Users.AnyAsync(u => u.UserName == username);
    }
}