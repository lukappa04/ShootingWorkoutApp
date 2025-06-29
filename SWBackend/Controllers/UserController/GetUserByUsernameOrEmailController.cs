using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWBackend.Attributes;
using SWBackend.DTO.UserDto;
using SWBackend.Enum;
using SWBackend.ServiceLayer.IService.IUserService;

namespace SWBackend.Controllers.UserController;

[Route("api/[controller]")]
[ApiController]
[Tags("User")]
public class GetUserByUsernameOrEmailController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<GetUserByUsernameOrEmailController> _logger;

    public GetUserByUsernameOrEmailController(IUserService userService, ILogger<GetUserByUsernameOrEmailController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost]
    [AuthorizeRoles(Role.Admin, Role.Coach)]
    public async Task<IActionResult> GetUserByUsernameOrEmail([FromBody] GetUserByUsernameOrEmailRequestDto request)
    {
        //TODO: Controllare come gestire l'erroe se un utente richiede il campo di un utente che non esiste, nello specifico se è stato eliminato.
        // try
        // {
        var user = await _userService.GetUserByUsernameOrEmailAsync(request);
            if (user == null) return NotFound();
            if (user.Role == "Admin")
            {
                _logger.LogError("You cant have this information");
                return Unauthorized();
            }
            return Ok(user);
        // }
        // catch (Exception ex)
        // {
        //     _logger.LogInformation(ex, "User does not exist");
        //     return BadRequest(new Exception(ex.Message));
        // }
    }

}

