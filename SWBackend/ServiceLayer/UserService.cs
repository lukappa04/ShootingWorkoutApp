using System.IO.Compression;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SWBackend.DTO.UserDto;
using SWBackend.Enum;
using SWBackend.Models.SignUp.Identity;
using SWBackend.Models.Token;
using SWBackend.RepositoryLayer.IRepository;
using SWBackend.RepositoryLayer.IRepository.User;
using SWBackend.ServiceLayer.Auth;
using SWBackend.ServiceLayer.IService.IUserService;
using SWBackend.ServiceLayer.Mail;



namespace SWBackend.ServiceLayer;


public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AppRole> _logger;
    private readonly IEmailerSender _emailSender;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenRepository _tokenRepository;

    public UserService(IUserRepository userRepository, UserManager<AppUser> userManager, IJwtService jwtService, ILogger<AppRole> logger, IEmailerSender emailerSender, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ITokenRepository tokenRepository)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _jwtService = jwtService;
        _logger = logger;
        _emailSender = emailerSender;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _tokenRepository = tokenRepository;
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
        var result = await _userRepository.CreateUserAsync(user, dto.Password);

        if (result.Succeeded)
        {
            // 1. Genera il token di conferma email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var baseUrl = _configuration["AppSettings:BackendBaseUrl"];

            // 2. Costruisci il link di conferma (potresti passare Host da fuori o usare config)
            var confirmationLink = $"{baseUrl}/api/ConfirmEmail/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            // 3. Invia l'email
            await _emailSender.SendEmailAsync(user.Email, "Conferma la tua email",
                $"<p>Clicca il seguente link per confermare la tua registrazione:</p><p><a href='{confirmationLink}'>Conferma Email</a></p>");
        }

        return result;
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
    public async Task<AuthenticationResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        string requestIp = httpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
        string userAgent = httpContext?.Request?.Headers["User-Agent"].ToString() ?? "unknown";


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

        //Se il 2FA è abilitato
        if (await _userManager.GetTwoFactorEnabledAsync(user))
        {
            var tokenTf = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            await _emailSender.SendEmailAsync(user.Email, "Codice di sicurezza",
                $"Il tuo codice di verifica 2FA è: {tokenTf}");
            return new AuthenticationResponseDto
            {
                Email = user.Email,
                Requires2FA = true
            };
        }

        //Altrimenti si continua con il login 
        // Genera il token JWT
        var token = _jwtService.GenerateToken(user);

        // Genera il refresh token
        var refreshToken = TokenMethod.GenerateRefreshToken(64);
        var refreshHash = TokenMethod.HashToken(refreshToken);
        
        await _tokenRepository.Save(new RefreshToken{
            UserId = user.Id,
            TokenHash = refreshHash,
            Expires = DateTime.UtcNow.AddDays(30),
            Created = DateTime.UtcNow,
            CreatedByIp = requestIp,
            UserAgent = userAgent
        });
        
        // user.RefreshToken = refreshToken;
        // user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(30);
        
       CookiesMethod.SetRefreshTokenCookie(_httpContextAccessor?.HttpContext?.Response, refreshToken, DateTime.Now.AddDays(30));

        //await _userManager.UpdateAsync(user);


        // Restituisce il DTO con il token e i dati utente base
        return new AuthenticationResponseDto
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Role = user.RoleCode,
            Token = token,
            RefreshToken = refreshToken,
            Requires2FA = false
        };

    }

    //TODO: Decidere cosa fare, se tenere la ricerca con i metodi di indentity ma non implementare il contains(), oppure farlo manuale con il contains.
    public async Task<UserResponseDto?> GetUserByUsernameOrEmailAsync(GetUserByUsernameOrEmailRequestDto request)
    {
        var query = request.UsernameOrEmailD.Trim().ToLower();

        var user = await _userManager.Users
            .Where(u => u.UserName.ToLower().Contains(query) || u.Email.ToLower().Contains(query))
            .FirstOrDefaultAsync();

        if (user == null)
        {
            _logger.LogInformation("Utente non trovato");
            return null;
        }

        return MapToDto(user);

        /*    
        var user = await _userManager.FindByNameAsync(request.UsernameOrEmailD) ?? await _userManager.FindByEmailAsync(request.UsernameOrEmailD);
        //TODO: Controllare come gestire l'erroe se un utente richiede il campo di un utente che non esiste, nello specifico se è stato eliminato.
        // if (user.DeleteDate != DateTime.MinValue)
        // {
        //     _logger.LogInformation("User does not exist");
        //     throw new Exception();
        // }
        return user == null ? null : MapToDto(user);
        */
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
        var result = alluser.Select(users => 
        users == null ? null : new UserResponseDto
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

    /// <summary>
    /// Metodo che controlla la valitià del 2FA Token. Si aspetta di ricevere l'email dell'utente interessato + il codice allegato.
    /// Se la mail esiste ed il codice è valido genera un JWT ed un RefreshToken per permettere il login
    /// </summary>
    /// <param name="request">Email / Code</param>
    /// <returns></returns>
    /// <exception cref="Exception">Se la mail non esiste / Se il codice passato non è valido</exception>
    public async Task<AuthenticationResponseDto> VerifyTwoFactorAsync(TwoFactoryVerifyRequestDto request)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        string requestIp = httpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
        string userAgent = httpContext?.Request?.Headers["User-Agent"].ToString() ?? "unknown";
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            _logger.LogError("Log: User not found!");
            throw new Exception("Ex: User not found!");
        }

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", request.Code);
        if (!isValid)
        {
            _logger.LogError("Log: Invalid code!");
            throw new Exception("Ex: Invalid code!");
        }
        
        //TODO: Fix this implementation
        var token = _jwtService.GenerateToken(user);
        // Genera il refresh token
        var refreshToken = TokenMethod.GenerateRefreshToken(64);
        var refreshHash = TokenMethod.HashToken(refreshToken);
        
        await _tokenRepository.Save(new RefreshToken{
            UserId = user.Id,
            TokenHash = refreshHash,
            Expires = DateTime.UtcNow.AddDays(30),
            Created = DateTime.UtcNow,
            CreatedByIp = requestIp,
            UserAgent = userAgent
        });
        
        CookiesMethod.SetRefreshTokenCookie(_httpContextAccessor?.HttpContext?.Response, refreshToken, DateTime.Now.AddDays(30));
        
        return new AuthenticationResponseDto
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Role = user.RoleCode,
            Token = token,
            RefreshToken = refreshToken,
            Requires2FA = false
        };
    }

    public async Task EnableTwoFactorAuthAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            _logger.LogError("Log: User not found!");
            throw new Exception("Ex: User not found!");
        }
        user.TwoFactorEnabled = true;
        await _userManager.UpdateAsync(user);
    }

    public async Task DisableTwoFactorAuthAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            _logger.LogError("Log: User not found!");
            throw new Exception("Ex: User not found!");
        }
        user.TwoFactorEnabled = false;
        await _userManager.UpdateAsync(user);
    }
    
    // Private
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
    
  
}

