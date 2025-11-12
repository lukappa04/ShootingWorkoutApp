using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWBackend.Attributes;
using swbackend.Db.Models.SignUp.Identity;
using SWBackend.ServiceLayer.IService.IUserService;

namespace SWBackend.Controllers.UserController;

[Route("api/[controller]")]
[ApiController]
[Tags("User")]
public class GetAllUserController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<GetAllUserController> _logger;

    public GetAllUserController(Logger<GetAllUserController> logger, UserManager<AppUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [HttpGet]
    //[AuthorizeRoles(Role.Admin)]
    public async Task<IActionResult> GetAllUser()
    {
        var result = await _userManager.Users.Where(u => u.DeleteDate == null).ToListAsync();
        return Ok(result);
    }
}

