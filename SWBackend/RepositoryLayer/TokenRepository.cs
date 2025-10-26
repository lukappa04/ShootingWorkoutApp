using Microsoft.EntityFrameworkCore;
using SWBackend.DataBase;
using SWBackend.Models.Token;
using SWBackend.RepositoryLayer.IRepository;

namespace SWBackend.RepositoryLayer;

public class TokenRepository : ITokenRepository
{
    private readonly SWDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenRepository(SWDbContext context, IHttpContextAccessor httpContextAccessor) 
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Save(RefreshToken token)
    {
        await _context.RefreshTokens.AddAsync(token);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken> GetByHash(string refreshToken)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.TokenHash == refreshToken);
    }

    public async Task RevokeAllRefreshTokensForUser(int userId)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        string requestIp = httpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
        var tokens = await _context.RefreshTokens
            .Where(t => t.UserId == userId && !t.Revoked)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.Revoked = true;
            token.RevokedAt = DateTime.UtcNow;
            token.RevokedByIp = requestIp; // o prendi IP da IHttpContextAccessor
        }

        await _context.SaveChangesAsync();
    }
}