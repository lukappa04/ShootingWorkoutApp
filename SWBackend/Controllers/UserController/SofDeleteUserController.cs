using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
public class SofDeleteUserController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<SofDeleteUserController> _logger;

    public SofDeleteUserController(ILogger<SofDeleteUserController> logger, UserManager<AppUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    /// <summary>
    /// EndPoint che esegue la SoftDelete di un Utente
    /// </summary>
    /// <param name="request">La request contiene due proprietà: UserId ed UsernameOrEmail,
    /// questo per non rischiare di eliminare l'utente sbagliato.</param>
    /// <returns>200 tutto ok / 404 not found</returns>
    [HttpDelete]
    [AuthorizeRoles(Role.Admin)]
    public async Task<IActionResult> Delete(DeleteUserRequestDto request)
    {
        // Verifico che l'Id dell utente esista
        var user = await _userManager.FindByIdAsync(request.UserIdD.ToString());
        if (user == null) return NotFound("User not found");
        
        // Verifica che Username o Email corrispondano
        var normalizedInput = request.UsernameOrEmailD.Trim().ToUpperInvariant();
        var isUsernameMatch = user.NormalizedUserName == normalizedInput;
        var isEmailMatch = user.NormalizedEmail == normalizedInput;
        
        if (!isUsernameMatch && !isEmailMatch)
        {
            _logger.LogError("Email and Id doesn't match");
            return NotFound("Email and Id doesn't match");
        }

        user.DeleteDate = DateTime.UtcNow;
        var result = await _userManager.UpdateAsync(user);

        return Ok(result);
    }
}

