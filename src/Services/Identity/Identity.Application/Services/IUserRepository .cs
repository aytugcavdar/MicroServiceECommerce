using BuildingBlocks.Core.Repositories;
using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Services;

public interface IUserRepository : IAsyncRepository<User, Guid>
{
    /// <summary>
    /// Email ile kullanıcı arar (roller dahil)
    /// </summary>
    Task<User?> GetByEmailWithRolesAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Username ile kullanıcı arar (roller dahil)
    /// </summary>
    Task<User?> GetByUserNameWithRolesAsync(string userName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kullanıcının aktif refresh token'larını getirir
    /// </summary>
    Task<List<RefreshToken>> GetActiveRefreshTokensAsync(Guid userId, CancellationToken cancellationToken = default);
}
