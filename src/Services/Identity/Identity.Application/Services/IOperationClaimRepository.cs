using BuildingBlocks.Core.Repositories;
using Identity.Domain.Entities;

namespace Identity.Application.Services;

public interface IOperationClaimRepository : IAsyncRepository<OperationClaim, Guid>
{
    /// <summary>
    /// Kullanıcının tüm rollerini getirir
    /// </summary>
    Task<List<OperationClaim>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// İsme göre rol bulur
    /// </summary>
    Task<OperationClaim?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
