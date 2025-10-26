using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWBackend.DataBase;
using SWBackend.ServiceLayer.Auth;

namespace SWBackend.Controllers.AuthUserController;
    [Route("api/[controller]")]
    [ApiController]
    [Tags("AuthUser")]
    public class LogoutController : ControllerBase
    {
        private readonly ILogger<LogoutController> _logger;
        private readonly SWDbContext _context;

        public LogoutController(ILogger<LogoutController> logger, SWDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                var hash = TokenMethod.HashToken(refreshToken);
                var tokenRecord = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == hash);
                if (tokenRecord != null)
                {
                    tokenRecord.Revoked = true;
                    tokenRecord.RevokedAt = DateTime.UtcNow;
                    tokenRecord.RevokedByIp = HttpContext.Connection.RemoteIpAddress?.ToString();
                    _context.RefreshTokens.Update(tokenRecord);
                    await _context.SaveChangesAsync();
                }
            }

            // cancella cookie
            Response.Cookies.Delete("refreshToken", new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Lax });

            return Ok();
        }
    }

