using BuildingBlocks.Core.Repositories;

namespace Payment.Application.Services;

public interface IPaymentRepository : IAsyncRepository<Domain.Entities.Payment, Guid>
{
    Task<Domain.Entities.Payment?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Domain.Entities.Payment>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
