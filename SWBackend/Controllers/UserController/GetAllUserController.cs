using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWBackend.Attributes;
using SWBackend.Enum;
using SWBackend.ServiceLayer.IService.IUserService;

namespace SWBackend.Controllers.UserController;

[Route("api/[controller]")]
[ApiController]
[Tags("User")]
public class GetAllUserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<GetAllUserController> _logger;

    public GetAllUserController(IUserService userService, ILogger<GetAllUserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    [AuthorizeRoles(Role.Admin)]
    public async Task<IActionResult> GetAllUser()
    {
        var result = await _userService.GetAllAsync();
        return Ok(result);
    }
}

