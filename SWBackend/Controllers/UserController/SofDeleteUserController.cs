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
public class SofDeleteUserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<SofDeleteUserController> _logger;

    public SofDeleteUserController(IUserService userService, ILogger<SofDeleteUserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpDelete]
    [AuthorizeRoles(Role.Admin)]
    public async Task<IActionResult> Delete(DeleteUserRequestDto request)
    {
        var user = await _userService.SoftDelete(request);
        return Ok(true);
    }
}

