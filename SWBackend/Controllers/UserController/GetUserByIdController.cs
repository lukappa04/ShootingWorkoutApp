using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWBackend.Attributes;
using SWBackend.DTO.UserDto;
using SWBackend.ServiceLayer.IService.IUserService;

namespace SWBackend.Controllers.UserController;

[Route("api/[controller]")]
[ApiController]
[Tags("User")]
public class GetUserByIdController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<GetUserByIdController> _logger;

    public GetUserByIdController(IUserService userService, ILogger<GetUserByIdController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost]
    [AuthorizeRoles(Enum.Role.Admin)]
    public async Task<IActionResult> GetUserById(GetUserByIdRequestDto request)
    {
        var result = await _userService.GetUserByIdAsync(request);
        if (result == null) return BadRequest();
        return Ok(result);
    }

}

