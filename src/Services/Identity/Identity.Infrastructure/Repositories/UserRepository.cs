using BuildingBlocks.Infrastructure.EntityFramework;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Infrastructure.Repositories;

public class UserRepository : EfRepositoryBase<User, Guid, IdentityDbContext>, IUserRepository
{
    public UserRepository(IdentityDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User?> GetByEmailWithRolesAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .Include(u => u.UserOperationClaims)
                .ThenInclude(uoc => uoc.OperationClaim)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByUserNameWithRolesAsync(
        string userName,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .Include(u => u.UserOperationClaims)
                .ThenInclude(uoc => uoc.OperationClaim)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
    }

    public async Task<List<RefreshToken>> GetActiveRefreshTokensAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId &&
                         rt.RevokedAt == null &&
                         rt.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(cancellationToken);
    }
}