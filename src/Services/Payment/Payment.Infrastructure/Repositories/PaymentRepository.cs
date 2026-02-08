using BuildingBlocks.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Payment.Application.Services;
using Payment.Infrastructure.Contexts;

namespace Payment.Infrastructure.Repositories;

public class PaymentRepository : EfRepositoryBase<Domain.Entities.Payment, Guid, PaymentDbContext>, IPaymentRepository
{
    public PaymentRepository(PaymentDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Domain.Entities.Payment?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await Query()
            .Where(p => p.DeletedDate == null)
            .FirstOrDefaultAsync(p => p.OrderId == orderId, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.Payment>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await Query()
            .Where(p => p.DeletedDate == null && p.UserId == userId)
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync(cancellationToken);
    }
}
