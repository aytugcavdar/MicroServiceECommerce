using BuildingBlocks.Core.Domain;
using BuildingBlocks.Core.Dynamic;
using BuildingBlocks.Core.Paging;
using BuildingBlocks.Core.Repositories;
using BuildingBlocks.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BuildingBlocks.Infrastructure.EntityFramework;

public class EfRepositoryBase<TEntity,TId, TDbContext>:IAsyncRepository<TEntity,TId>
    where TEntity : Entity<TId>
    where TDbContext : DbContext
{
    protected readonly TDbContext _dbContext;
    public EfRepositoryBase(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<TEntity> Query() => _dbContext.Set<TEntity>();

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        entity.CreatedDate = DateTime.Now;
        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities)
    {
        foreach (var entity in entities)
            entity.CreatedDate = DateTime.UtcNow;

        await _dbContext.AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
        return entities;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false)
    {
        if (permanent)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        entity.DeletedDate = DateTime.Now;
        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        var queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (!withDeleted)
            queryable = queryable.AsNoTracking();
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<Paginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, bool withDeleted = false, bool enableTracking = true, int index = 0, int size = 10, CancellationToken cancellationToken = default)
    {
        var queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();

        if (include != null)
            queryable = include(queryable);

        if (!withDeleted)
            queryable = queryable.AsNoTracking();

        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return await orderBy(queryable).ToPaginateAsync(index, size, cancellationToken);

        return await queryable.ToPaginateAsync(index, size, cancellationToken);
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        entity.UpdatedDate = DateTime.UtcNow;
        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<Paginate<TEntity>> GetListByDynamicAsync(
    DynamicQuery dynamic,
    Expression<Func<TEntity, bool>>? predicate = null,
    Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
    bool withDeleted = false,
    bool enableTracking = true,
    int index = 0,
    int size = 10,
    CancellationToken cancellationToken = default)
    {
        var queryable = Query().ToDynamic(dynamic);

        if (!enableTracking)
            queryable = queryable.AsNoTracking();

        if (include != null)
            queryable = include(queryable);

        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();

        if (predicate != null)
            queryable = queryable.Where(predicate);

        return await queryable.ToPaginateAsync(index, size, cancellationToken);
    }

    public Task<Paginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, bool withDeleted = false, bool enableTracking = true, int index = 0, int size = 10, CancellationToken cancellationToken = default, Func<object, object> include = null)
    {
        throw new NotImplementedException();
    }
}
