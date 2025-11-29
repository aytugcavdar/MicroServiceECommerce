using BuildingBlocks.Infrastructure.EntityFramework;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository: EfRepositoryBase<Product, Guid, CatalogDbContext>
{
    public ProductRepository(CatalogDbContext dbContext) : base(dbContext)
    {
    }
}
