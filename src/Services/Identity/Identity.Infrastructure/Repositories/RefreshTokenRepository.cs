using BuildingBlocks.Infrastructure.EntityFramework;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Infrastructure.Contexts;

namespace Identity.Infrastructure.Repositories;

public class RefreshTokenRepository : EfRepositoryBase<RefreshToken, Guid, IdentityDbContext>, IRefreshTokenRepository
{
    public RefreshTokenRepository(IdentityDbContext dbContext) : base(dbContext)
    {
    }

    public Task DeleteExpiredTokensAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
