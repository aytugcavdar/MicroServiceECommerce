using BuildingBlocks.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Services.Repositories;

public interface IOrderRepository : IAsyncRepository<Domain.Entities.Order, Guid>
{
}
