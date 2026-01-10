using BuildingBlocks.Core.Repositories;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Services;

public interface ICategoryRepository: IAsyncRepository<Category,Guid>
{
    Task<int> GetProductCountAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<bool> HasProductsAsync(Guid categoryId, CancellationToken cancellationToken = default);
}
