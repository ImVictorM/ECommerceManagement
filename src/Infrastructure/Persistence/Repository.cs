using System.Linq.Expressions;
using Application.Common.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace Infrastructure.Persistence;

/// <summary>
/// Defines a generic repository to handle aggregate interactions.
/// </summary>
/// <typeparam name="TEntity">The aggregate type.</typeparam>
/// <typeparam name="TEntityId">The aggregate id type.</typeparam>
public sealed class Repository<TEntity, TEntityId> : IRepository<TEntity, TEntityId>
    where TEntity : AggregateRoot<TEntityId>
    where TEntityId : notnull
{
    /// <summary>
    /// The database context.
    /// </summary>
    private readonly ECommerceDbContext _context;
    /// <summary>
    /// The aggregate db set.
    /// </summary>
    private readonly DbSet<TEntity> _dbSet;

    /// <summary>
    /// Initiates a new instance of the <see cref="Repository{TEntity, TEntityId}"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public Repository(ECommerceDbContext dbContext)
    {
        _context = dbContext;
        _dbSet = _context.Set<TEntity>();
    }

    /// <inheritdoc/>
    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>>? filter = null)
    {

        if (filter != null)
        {
            return await _dbSet.Where(filter).ToListAsync();
        }

        return await _dbSet.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<TEntity?> FindByIdAsync(TEntityId id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <inheritdoc/>
    public async Task<TEntity?> FindOneOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _dbSet.FirstOrDefaultAsync(filter);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> FindSatisfyingAsync(ISpecificationQuery<TEntity> specification, int? limit = null)
    {
        var query = _dbSet.Where(specification.Criteria);

        if (limit is int limitValue)
        {
            query = query.Take(limitValue);
        }

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<TEntity?> FindFirstSatisfyingAsync(ISpecificationQuery<TEntity> specification)
    {
        return await _dbSet.Where(specification.Criteria).FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<TEntity?> FindByIdSatisfyingAsync(TEntityId id, ISpecificationQuery<TEntity> specification)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "entity");

        var idProperty = Expression.Property(parameter, nameof(AggregateRoot<TEntityId>.Id));
        var idValue = Expression.Constant(id);
        var idFilter = Expression.Equal(idProperty, idValue);

        var specFilter = Expression.Invoke(specification.Criteria, parameter);
        var combinedFilter = Expression.AndAlso(idFilter, specFilter);

        var combinedExpression = Expression.Lambda<Func<TEntity, bool>>(combinedFilter, parameter);

        return await _dbSet.FirstOrDefaultAsync(combinedExpression);
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(TEntityId id)
    {
        var entity = await _dbSet.FindAsync(id);

        if (entity == null)
        {
            return;
        }

        if (entity is ISoftDeletable softDeletable)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            softDeletable.MakeInactive();

            _dbSet.Update(entity);
        }
        else
        {
            _dbSet.Remove(entity);
        }
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(TEntity entity)
    {
        var existingEntity = await _dbSet.FindAsync(entity.Id);

        if (existingEntity == null)
        {
            return;
        }

        _context.Entry(existingEntity).CurrentValues.SetValues(entity);
    }
}
