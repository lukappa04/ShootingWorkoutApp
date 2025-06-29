using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWBackend.DTO.UserDto;
using SWBackend.ServiceLayer.IService.IUserService;

namespace SWBackend.Controllers.AuthUserController;

[Route("api/[controller]")]
[ApiController]
[Tags("AuthUser")]
public class LoginController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<LoginController> _logger;
    public LoginController(IUserService userService, ILogger<LoginController> logger)
    {
        _userService = userService;
        _logger = logger;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        try
        {
            var authResponse = await _userService.LoginAsync(request);
            return Ok(authResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LoginFailed");
            return Unauthorized(new { message = ex.Message });
        }
    }
}

