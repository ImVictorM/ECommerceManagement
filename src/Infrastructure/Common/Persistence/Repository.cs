using Application.Common.Persistence;

using SharedKernel.Interfaces;
using SharedKernel.Models;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Common.Persistence;

/// <summary>
/// Defines a generic repository to handle aggregate interactions.
/// </summary>
/// <typeparam name="TEntity">The aggregate type.</typeparam>
/// <typeparam name="TEntityId">The aggregate id type.</typeparam>
public sealed class Repository<TEntity, TEntityId> : IRepository<TEntity, TEntityId>
    where TEntity : AggregateRoot<TEntityId>
    where TEntityId : notnull
{
    private readonly ECommerceDbContext _context;
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
    public async Task<TEntity> FirstAsync()
    {
        return await _dbSet.FirstAsync();
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
    public void RemoveOrDeactivate(TEntity entity)
    {
        if (entity is IActivatable activatable)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            activatable.Deactivate();

            _dbSet.Update(entity);
        }
        else
        {
            _dbSet.Remove(entity);
        }
    }
}
