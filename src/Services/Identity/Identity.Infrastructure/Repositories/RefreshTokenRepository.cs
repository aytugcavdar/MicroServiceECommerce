using BuildingBlocks.Infrastructure.EntityFramework;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public class RefreshTokenRepository : EfRepositoryBase<RefreshToken, Guid, IdentityDbContext>, IRefreshTokenRepository
{
    public RefreshTokenRepository(IdentityDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<RefreshToken?> GetByTokenAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }

    public async Task<List<RefreshToken>> GetActiveTokensByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId &&
                         rt.RevokedAt == null &&
                         rt.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteExpiredTokensAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var expiredTokens = await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId &&
                         (rt.RevokedAt != null || rt.ExpiresAt <= DateTime.UtcNow))
            .ToListAsync(cancellationToken);

        _dbContext.RefreshTokens.RemoveRange(expiredTokens);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
