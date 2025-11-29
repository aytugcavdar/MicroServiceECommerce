using BuildingBlocks.Core.Domain;
using BuildingBlocks.Core.Dynamic;
using BuildingBlocks.Core.Paging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BuildingBlocks.Core.Repositories;

public interface IAsyncRepository<TEntity, TId> : IQuery<TEntity>
    where TEntity : Entity<TId>
{
    Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default);

    Task<Paginate<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        int index = 0,
        int size = 10,
        CancellationToken cancellationToken = default);

    Task<Paginate<TEntity>> GetListByDynamicAsync(
    DynamicQuery dynamic,
    Expression<Func<TEntity, bool>>? predicate = null,
    Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
    bool withDeleted = false,
    bool enableTracking = true,
    int index = 0,
    int size = 10,
    CancellationToken cancellationToken = default);

    Task<TEntity> AddAsync(TEntity entity);
    Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false);
}
