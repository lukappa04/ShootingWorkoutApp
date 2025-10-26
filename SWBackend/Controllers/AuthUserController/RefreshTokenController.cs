using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWBackend.DataBase;
using SWBackend.DTO.UserDto;
using SWBackend.Models.SignUp.Identity;
using SWBackend.Models.Token;
using SWBackend.RepositoryLayer.IRepository;
using SWBackend.RepositoryLayer.IRepository.User;
using SWBackend.ServiceLayer.Auth;
using SWBackend.ServiceLayer.IService.IUserService;

namespace SWBackend.Controllers.AuthUserController;

[Route("api/[controller]")]
[ApiController]
[Tags("AuthUser")]
public class RefreshTokenController : ControllerBase
{
    //TODO: RICONTROLLARE
    private readonly ILogger<RefreshTokenController> _logger;
    private readonly IJwtService _jwtService;
    private readonly ITokenRepository _tokenRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly SWDbContext _context;

    public RefreshTokenController(ILogger<RefreshTokenController> logger, IJwtService jwtService, UserManager<AppUser> userManager, SWDbContext context, ITokenRepository tokenRepository)
    {
        _logger = logger;
        _jwtService = jwtService;
        _userManager = userManager;
        _context = context;
        _tokenRepository = tokenRepository;
    }

    // [HttpPost]
    // public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto request)
    // {
    //     var user = await _userManager.Users
    //     .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
    //
    //     if (user == null || user.RefreshTokenExpiration < DateTime.UtcNow)
    //     {
    //         return Unauthorized("Token non valido o scaduto");
    //     }
    //
    //     // Genera nuovi token
    //     var newAccessToken = _jwtService.GenerateToken(user);
    //     var newRefreshToken = _jwtService.GenerateRefreshToken();
    //
    //     user.RefreshToken = newRefreshToken;
    //     user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(30);
    //     await _userManager.UpdateAsync(user);
    //
    //     return Ok(new AuthenticationResponseDto
    //     {
    //         Id = user.Id,
    //         Username = user.UserName ?? string.Empty,
    //         Email = user.Email ?? string.Empty,
    //         Role = user.RoleCode,
    //         Token = newAccessToken,
    //         RefreshToken = newRefreshToken
    //     });
    // }
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        //Recupera il token dal cookie, se non esiste l'utente non è autenticato
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            return Unauthorized("No refresh token.");

        //Calcola l'hash del token ricevuto dal db
        var refreshHash = TokenMethod.HashToken(refreshToken);
        var tokenRecord = await _context.RefreshTokens
            .Include(rt => rt.User) // se vuoi i dati user
            .FirstOrDefaultAsync(rt => rt.TokenHash == refreshHash);

        // Token non trovato
        if (tokenRecord == null)
        {
            // possibile replay attack: potresti voler revocare tutti i token dell'ip/userAgent se sospetto
            return Unauthorized("Token non valido.");
        }

        if (tokenRecord.Revoked)
        {
            // Rilevato riutilizzo di token revocato -> azione di sicurezza
            // revoca tutti i token dell'utente come misura drastica:
            await _tokenRepository.RevokeAllRefreshTokensForUser(tokenRecord.UserId);
            return Unauthorized("Token revocato. Effettua nuovamente il login.");
        }

        if (tokenRecord.IsExpired)
        {
            return Unauthorized("Token scaduto");
        }

        // OK: rotazione
        var user = await _userManager.FindByIdAsync(tokenRecord.UserId.ToString());

        // crea nuovo refresh token
        var newPlainRefresh = TokenMethod.GenerateRefreshToken();
        var newHash = TokenMethod.HashToken(newPlainRefresh);

        // marca il vecchio come revocato e setta replacedBy
        tokenRecord.Revoked = true;
        tokenRecord.RevokedAt = DateTime.UtcNow;
        tokenRecord.RevokedByIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        tokenRecord.ReplacedByTokenHash = newHash;
        _context.RefreshTokens.Update(tokenRecord);

        // aggiungi il nuovo record
        var newTokenRecord = new RefreshToken
        {
            UserId = tokenRecord.UserId,
            TokenHash = newHash,
            Expires = DateTime.UtcNow.AddDays(30),
            Created = DateTime.UtcNow,
            CreatedByIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
            UserAgent = Request.Headers["User-Agent"].ToString()
        };
        _context.RefreshTokens.Add(newTokenRecord);
        await _context.SaveChangesAsync();

        // genera nuova access token
        var newJwt = _jwtService.GenerateToken(user);

        // setta nuovo cookie (HttpOnly)
        CookiesMethod.SetRefreshTokenCookie(Response, newPlainRefresh, newTokenRecord.Expires);

        return Ok(new { token = newJwt, expiresIn = 900 });
    }

}

