using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
public class GetUserByUsernameOrEmailController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<GetUserByUsernameOrEmailController> _logger;

    public GetUserByUsernameOrEmailController(ILogger<GetUserByUsernameOrEmailController> logger, UserManager<AppUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [HttpPost]
    [AuthorizeRoles(Role.Admin, Role.Coach)]
    public async Task<IActionResult> GetUserByUsernameOrEmail([FromBody] GetUserByUsernameOrEmailRequestDto request)
    {
        //TODO: Controllare come gestire l'erroe se un utente richiede il campo di un utente che non esiste,
        //nello specifico se è stato eliminato.
      
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == request.UsernameOrEmailD 
                                                                     || u.NormalizedEmail == request.UsernameOrEmailD);
        if (user == null) return NotFound("User not found");
        return Ok(user);
        
    }

}

