using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SWBackend.Models.Token;

namespace SWBackend.RepositoryLayer.IRepository;

public interface ITokenRepository
{
    Task Save(RefreshToken token);
    Task<RefreshToken> GetByHash(string refreshToken);
    Task RevokeAllRefreshTokensForUser(int userId);
    
}