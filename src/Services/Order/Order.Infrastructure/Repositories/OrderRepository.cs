using BuildingBlocks.Infrastructure.EntityFramework;
using Order.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Infrastructure.Repositories;

public class OrderRepository : EfRepositoryBase<Domain.Entities.Order, Guid, OrderDbContext>, IOrderRepository
{
    public OrderRepository(OrderDbContext context) : base(context)
    {
    }
}
