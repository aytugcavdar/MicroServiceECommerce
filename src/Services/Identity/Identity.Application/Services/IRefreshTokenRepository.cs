using BuildingBlocks.Core.Repositories;
using Identity.Domain.Entities;

namespace Identity.Application.Services;

public interface IRefreshTokenRepository : IAsyncRepository<RefreshToken, Guid>
{
    /// <summary>
    /// Token string'i ile refresh token bulur
    /// </summary>
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kullanıcının tüm aktif refresh token'larını getirir
    /// </summary>
    Task<List<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kullanıcının eski/süresi dolmuş refresh token'larını siler
    /// </summary>
    Task DeleteExpiredTokensAsync(Guid userId, CancellationToken cancellationToken = default);
}
