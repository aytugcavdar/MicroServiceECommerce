using BuildingBlocks.Core.Repositories;
using Identity.Domain.Entities;

namespace Identity.Application.Services;

public interface IUserOperationClaimRepository : IAsyncRepository<UserOperationClaim, Guid>
{
    /// <summary>
    /// Kullanıcının tüm rollerini getirir (OperationClaim dahil)
    /// </summary>
    /// <param name="userId">Kullanıcı ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kullanıcının rolleri</returns>
    Task<List<UserOperationClaim>> GetUserClaimsWithDetailsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Kullanıcının belirli bir role sahip olup olmadığını kontrol eder
    /// </summary>
    /// <param name="userId">Kullanıcı ID</param>
    /// <param name="operationClaimId">Rol ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kullanıcı bu role sahipse true</returns>
    Task<bool> HasClaimAsync(
        Guid userId,
        Guid operationClaimId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Kullanıcının belirli bir rol adına sahip olup olmadığını kontrol eder
    /// </summary>
    /// <param name="userId">Kullanıcı ID</param>
    /// <param name="claimName">Rol adı</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Kullanıcı bu role sahipse true</returns>
    Task<bool> HasClaimByNameAsync(
        Guid userId,
        string claimName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Kullanıcıya rol atar
    /// </summary>
    /// <param name="userId">Kullanıcı ID</param>
    /// <param name="operationClaimId">Rol ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Oluşturulan UserOperationClaim</returns>
    Task<UserOperationClaim> AssignClaimToUserAsync(
        Guid userId,
        Guid operationClaimId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Kullanıcıdan rol kaldırır
    /// </summary>
    /// <param name="userId">Kullanıcı ID</param>
    /// <param name="operationClaimId">Rol ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Silinen kayıt sayısı</returns>
    Task<int> RemoveClaimFromUserAsync(
        Guid userId,
        Guid operationClaimId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Kullanıcının tüm rollerini kaldırır
    /// </summary>
    /// <param name="userId">Kullanıcı ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Silinen kayıt sayısı</returns>
    Task<int> RemoveAllClaimsFromUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirli bir role sahip tüm kullanıcıları getirir
    /// </summary>
    /// <param name="operationClaimId">Rol ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Bu role sahip kullanıcılar</returns>
    Task<List<UserOperationClaim>> GetUsersByClaimAsync(
        Guid operationClaimId,
        CancellationToken cancellationToken = default);
}