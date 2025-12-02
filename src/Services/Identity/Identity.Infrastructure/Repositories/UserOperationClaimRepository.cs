using BuildingBlocks.Infrastructure.EntityFramework;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Infrastructure.Contexts;

namespace Identity.Infrastructure.Repositories;

public class UserOperationClaimRepository : EfRepositoryBase<UserOperationClaim, Guid, IdentityDbContext>, IUserOperationClaimRepository
{
    public UserOperationClaimRepository(IdentityDbContext dbContext) : base(dbContext)
    {
    }

    public Task<UserOperationClaim> AssignClaimToUserAsync(Guid userId, Guid operationClaimId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserOperationClaim>> GetUserClaimsWithDetailsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserOperationClaim>> GetUsersByClaimAsync(Guid operationClaimId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasClaimAsync(Guid userId, Guid operationClaimId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasClaimByNameAsync(Guid userId, string claimName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> RemoveAllClaimsFromUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> RemoveClaimFromUserAsync(Guid userId, Guid operationClaimId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}