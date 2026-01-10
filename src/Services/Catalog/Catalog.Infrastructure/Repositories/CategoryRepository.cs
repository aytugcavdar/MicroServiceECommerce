using BuildingBlocks.CrossCutting.Exceptions.types;
using BuildingBlocks.Infrastructure.EntityFramework;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Infrastructure.Repositories;

public class CategoryRepository: EfRepositoryBase<Category, Guid, CatalogDbContext>, ICategoryRepository
{
    public CategoryRepository(CatalogDbContext dbContext) : base(dbContext)
    {
        
    }

    public async Task<int> GetProductCountAsync(
    Guid categoryId,
    CancellationToken cancellationToken = default)
    {
        return await _dbContext.Categories
            .Where(c => c.Id == categoryId)
            .SelectMany(c => c.Products)
            .CountAsync(cancellationToken);
    }

    public async Task<bool> HasProductsAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Categories
            .Where(c => c.Id == categoryId)
            .AnyAsync(c => c.Products.Any(), cancellationToken);
    }

    
}
