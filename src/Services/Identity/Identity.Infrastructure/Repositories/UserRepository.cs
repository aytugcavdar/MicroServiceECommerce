using BuildingBlocks.Infrastructure.EntityFramework;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Infrastructure.Repositories;

public class UserRepository : EfRepositoryBase<User, Guid, IdentityDbContext>, IUserRepository
{
    public UserRepository(IdentityDbContext dbContext) : base(dbContext)
    {
    }

    public Task<List<RefreshToken>> GetActiveRefreshTokensAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByEmailWithRolesAsync(string email, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByUserNameWithRolesAsync(string userName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
