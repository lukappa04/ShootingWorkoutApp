using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWBackend.Attributes;
using SWBackend.Attributes.AuthorizeRole;
using SWBackend.DTO.UserDto;
using SWBackend.Enum;
using SWBackend.ServiceLayer.IService.IUserService;

namespace SWBackend.Controllers.UserController;

[Route("api/[controller]")]
[ApiController]
[Tags("User")]
public class ChangePasswordController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<ChangePasswordController> _logger;
    public ChangePasswordController(IUserService userService, ILogger<ChangePasswordController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost]
    [AuthorizeRoles(Role.Admin, Role.Coach, Role.User)]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto request)
    {
        var username = GetLoggedInUsername();
        var result = await _userService.ChangePasswordAsync(username, request);

        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok("Password cambiata con successo");
    }
    private string GetLoggedInUsername()
    {
        var usernameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        if (usernameClaim == null)
        {
            _logger.LogError("Log: Token JWT invalid: USERNAME IS MISSING");
            throw new UnauthorizedAccessException("Ex: Token JWT invalid: Username is missing");
        }
        return usernameClaim.Value;
    }
}

