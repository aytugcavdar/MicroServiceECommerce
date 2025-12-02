using BuildingBlocks.Infrastructure.EntityFramework;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Infrastructure.Contexts;

namespace Identity.Infrastructure.Repositories;

public class OperationClaimRepository : EfRepositoryBase<OperationClaim, Guid, IdentityDbContext>, IOperationClaimRepository
{
    public OperationClaimRepository(IdentityDbContext dbContext) : base(dbContext)
    {
    }

    public Task<OperationClaim?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<OperationClaim>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
