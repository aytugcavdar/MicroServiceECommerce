using BuildingBlocks.Core.Repositories;
using Order.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Services.Repositories;

public interface IOrderRepository : IAsyncRepository<Domain.Entities.Order, Guid>
{
    Task<List<Domain.Entities.Order>> GetUserOrdersAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<Domain.Entities.Order?> GetOrderWithItemsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    Task<List<Domain.Entities.Order>> GetOrdersByStatusAsync(
        OrderStatus status,
        DateTime? fromDate = null,
        CancellationToken cancellationToken = default);

    Task<decimal> GetUserTotalSpendingAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<bool> HasPendingOrdersAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
