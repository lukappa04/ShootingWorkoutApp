using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWBackend.Attributes;
using SWBackend.Attributes.AuthorizeRole;
using swbackend.Db.Enum;
using swbackend.Db.Models.SignUp.Identity;
using SWBackend.DTO.UserDto;
using SWBackend.ServiceLayer.IService.IUserService;

namespace SWBackend.Controllers.UserController;

[Route("api/[controller]")]
[ApiController]
[Tags("User")]
public class GetUserByIdController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<GetUserByIdController> _logger;

    public GetUserByIdController(ILogger<GetUserByIdController> logger, UserManager<AppUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [HttpPost]
    [AuthorizeRoles(Role.Admin)]
    public async Task<IActionResult> GetUserById(GetUserByIdRequestDto request)
    {
        var result = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UserIdD);
        if (result == null) return BadRequest("User not found");
        return Ok(result);
    }

}

