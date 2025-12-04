using BuildingBlocks.Infrastructure.EntityFramework;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public class UserOperationClaimRepository : EfRepositoryBase<UserOperationClaim, Guid, IdentityDbContext>, IUserOperationClaimRepository
{
    public UserOperationClaimRepository(IdentityDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<UserOperationClaim>> GetUserClaimsWithDetailsAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserOperationClaims
            .Include(uoc => uoc.OperationClaim)
            .Include(uoc => uoc.User)
            .Where(uoc => uoc.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasClaimAsync(
        Guid userId,
        Guid operationClaimId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserOperationClaims
            .AnyAsync(uoc => uoc.UserId == userId &&
                            uoc.OperationClaimId == operationClaimId,
                     cancellationToken);
    }

    public async Task<bool> HasClaimByNameAsync(
        Guid userId,
        string claimName,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserOperationClaims
            .Include(uoc => uoc.OperationClaim)
            .AnyAsync(uoc => uoc.UserId == userId &&
                            uoc.OperationClaim.Name == claimName,
                     cancellationToken);
    }

    public async Task<UserOperationClaim> AssignClaimToUserAsync(
        Guid userId,
        Guid operationClaimId,
        CancellationToken cancellationToken = default)
    {
        // Önce zaten var mı kontrol et
        var exists = await HasClaimAsync(userId, operationClaimId, cancellationToken);
        if (exists)
            throw new InvalidOperationException("User already has this claim");

        var userOperationClaim = new UserOperationClaim(userId, operationClaimId);
        await _dbContext.UserOperationClaims.AddAsync(userOperationClaim, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return userOperationClaim;
    }

    public async Task<int> RemoveClaimFromUserAsync(
        Guid userId,
        Guid operationClaimId,
        CancellationToken cancellationToken = default)
    {
        var userClaim = await _dbContext.UserOperationClaims
            .FirstOrDefaultAsync(uoc => uoc.UserId == userId &&
                                       uoc.OperationClaimId == operationClaimId,
                                cancellationToken);

        if (userClaim == null)
            return 0;

        _dbContext.UserOperationClaims.Remove(userClaim);
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> RemoveAllClaimsFromUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var userClaims = await _dbContext.UserOperationClaims
            .Where(uoc => uoc.UserId == userId)
            .ToListAsync(cancellationToken);

        _dbContext.UserOperationClaims.RemoveRange(userClaims);
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<UserOperationClaim>> GetUsersByClaimAsync(
        Guid operationClaimId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserOperationClaims
            .Include(uoc => uoc.User)
            .Include(uoc => uoc.OperationClaim)
            .Where(uoc => uoc.OperationClaimId == operationClaimId)
            .ToListAsync(cancellationToken);
    }
}