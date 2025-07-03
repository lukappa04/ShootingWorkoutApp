using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWBackend.DTO.UserDto;
using SWBackend.Models.SignUp.Identity;
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
    private readonly UserManager<AppUser> _userManager;

    public RefreshTokenController(ILogger<RefreshTokenController> logger, IJwtService jwtService, UserManager<AppUser> userManager)
    {
        _logger = logger;
        _jwtService = jwtService;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto request)
    {
        var user = await _userManager.Users
        .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);

        if (user == null || user.RefreshTokenExpiration < DateTime.UtcNow)
        {
            return Unauthorized("Token non valido o scaduto");
        }

        // Genera nuovi token
        var newAccessToken = _jwtService.GenerateToken(user);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(30);
        await _userManager.UpdateAsync(user);

        return Ok(new AuthenticationResponseDto
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Role = user.RoleCode,
            Token = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }
}

