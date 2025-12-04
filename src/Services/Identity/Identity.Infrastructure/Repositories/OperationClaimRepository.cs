using BuildingBlocks.Infrastructure.EntityFramework;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public class OperationClaimRepository : EfRepositoryBase<OperationClaim, Guid, IdentityDbContext>, IOperationClaimRepository
{
    public OperationClaimRepository(IdentityDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<OperationClaim?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.OperationClaims
            .FirstOrDefaultAsync(oc => oc.Name == name, cancellationToken);
    }

    public async Task<List<OperationClaim>> GetUserRolesAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserOperationClaims
            .Where(uoc => uoc.UserId == userId)
            .Include(uoc => uoc.OperationClaim)
            .Select(uoc => uoc.OperationClaim)
            .ToListAsync(cancellationToken);
    }
}
