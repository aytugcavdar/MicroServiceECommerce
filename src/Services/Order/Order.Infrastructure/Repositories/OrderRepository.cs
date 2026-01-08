using BuildingBlocks.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Order.Application.Services.Repositories;
using Order.Domain.Enums;
using Order.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Infrastructure.Repositories;

public class OrderRepository : EfRepositoryBase<Domain.Entities.Order, Guid, OrderDbContext>, IOrderRepository
{
    public OrderRepository(OrderDbContext context) : base(context) { }

    public async Task<List<Domain.Entities.Order>> GetUserOrdersAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Domain.Entities.Order?> GetOrderWithItemsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
    }

    public async Task<List<Domain.Entities.Order>> GetOrdersByStatusAsync(
        OrderStatus status,
        DateTime? fromDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Orders.Where(o => o.Status == status);

        if (fromDate.HasValue)
        {
            query = query.Where(o => o.CreatedDate >= fromDate.Value);
        }

        return await query
            .Include(o => o.OrderItems)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetUserTotalSpendingAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .Where(o => o.UserId == userId && o.Status == OrderStatus.Completed)
            .SumAsync(o => o.TotalPrice, cancellationToken);
    }

    public async Task<bool> HasPendingOrdersAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .AnyAsync(o => o.UserId == userId &&
                          o.Status != OrderStatus.Completed &&
                          o.Status != OrderStatus.Canceled &&
                          o.Status != OrderStatus.Failed,
                     cancellationToken);
    }
}
