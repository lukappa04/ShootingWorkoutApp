using System.IO.Compression;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using SWBackend.DTO.UserDto;
using SWBackend.Enum;
using SWBackend.Models.SignUp.Identity;
using SWBackend.RepositoryLayer.IRepository.User;
using SWBackend.ServiceLayer.Auth;
using SWBackend.ServiceLayer.IService.IUserService;

namespace SWBackend.ServiceLayer;


public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AppRole> _logger;

    public UserService(IUserRepository userRepository, UserManager<AppUser> userManager, IJwtService jwtService, ILogger<AppRole> logger)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<bool> CheckPasswordAsync(LoginRequestDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.UsernameOrEmailD);
        if (user == null) return false;
        return await _userRepository.CheckPasswordAsync(user, dto.Password);
    }

    public async Task<IdentityResult> CreateUserAsync(RegisterUserRequestDto dto)
    {
        var user = new AppUser
        {
            UserName = dto.UsernameD,
            Email = dto.EmailD,
            Name = dto.NameD,
            Surname = dto.SurnameD,
            Age = dto.AgeD,
            BirthDay = dto.BirthDay,
            //RoleCode = Enum.Parse<Role>(dto.) // gestisci il parsing con attenzione
        };

        return await _userRepository.CreateUserAsync(user, dto.Password);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _userRepository.EmailExistsAsync(email);
    }

    public async Task<UserResponseDto?> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user == null ? null : MapToDto(user);
    }

    public async Task<UserResponseDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : MapToDto(user);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _userRepository.UsernameExistsAsync(username);
    }
    private static UserResponseDto MapToDto(AppUser user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Name = user.Name,
            Surname = user.Surname,
            Age = user.Age,
            BirthDay = user.BirthDay,
            Role = user.RoleCode.ToString()
        };
    }
    public async Task<AuthenticationResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await _userManager.FindByNameAsync(request.UsernameOrEmailD)
          ?? await _userManager.FindByEmailAsync(request.UsernameOrEmailD);

        if (user == null)
        {
            _logger.LogError("Utente non trovato.");
            throw new Exception("Utente non trovato.");
        }

        // Verifica la password
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            _logger.LogError("Password errata");
            throw new Exception("Password errata.");
        }

        // Genera il token JWT
        var token = _jwtService.GenerateToken(user);

        // Restituisce il DTO con il token e i dati utente base
        //TODO: capire pechè Non prende bene il role e mi da errori
        return new AuthenticationResponseDto
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Role = user.RoleCode,
            Token = token
        };

    }

    public async Task<UserResponseDto?> GetUserByUsernameOrEmailAsync(GetUserByUsernameOrEmailRequestDto request)
    {
        var user = await _userManager.FindByNameAsync(request.UsernameOrEmailD) ?? await _userManager.FindByEmailAsync(request.UsernameOrEmailD);
        //TODO: Controllare come gestire l'erroe se un utente richiede il campo di un utente che non esiste, nello specifico se è stato eliminato.
        // if (user.DeleteDate != DateTime.MinValue)
        // {
        //     _logger.LogInformation("User does not exist");
        //     throw new Exception();
        // }
        return user == null ? null : MapToDto(user);
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(GetUserByIdRequestDto request)
    {
        var user = await _userManager.FindByIdAsync(request.UserIdD.ToString());

        return user == null ? null : MapToDto(user);

    }

    public async Task<bool> SoftDelete(DeleteUserRequestDto request)
    {
        var user = await _userManager.FindByIdAsync(request.UserIdD.ToString());
        if (user == null) return false;

        // Verifica che Username o Email corrispondano
        var normalizedInput = request.UsernameOrEmailD.Trim().ToUpperInvariant();
        var isUsernameMatch = user.NormalizedUserName == normalizedInput;
        var isEmailMatch = user.NormalizedEmail == normalizedInput;

        if (!isUsernameMatch && !isEmailMatch)
        {
            _logger.LogError("Email and Id doesn't match");
            return false;
        }

        user.DeleteDate = DateTime.UtcNow;
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    public async Task<List<UserResponseDto?>> GetAllAsync()
    {
        var alluser = await _userRepository.GetAllUserAsync();
        var result = alluser.Select(users => new UserResponseDto
        {
            Id = users.Id,
            Username = users.UserName ?? string.Empty,
            Email = users.Email ?? string.Empty,
            Role = users.RoleCode.ToString(),
        }).ToList();

        return result;
    }
    

    
    public async Task<IdentityResult> ChangePasswordAsync(string username, ChangePasswordRequestDto request)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            _logger.LogError("Log: User not found!");
            throw new Exception("Ex: User not found!");
        }
        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        return result;
    }
}

