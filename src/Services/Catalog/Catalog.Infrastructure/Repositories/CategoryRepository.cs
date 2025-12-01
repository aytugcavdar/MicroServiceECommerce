using BuildingBlocks.Infrastructure.EntityFramework;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Infrastructure.Repositories;

public class CategoryRepository: EfRepositoryBase<Category, Guid, CatalogDbContext>, ICategoryRepository
{
    public CategoryRepository(CatalogDbContext dbContext) : base(dbContext)
    {
        
    }
}
