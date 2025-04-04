using SharedKernel.Interfaces;
using SharedKernel.Models;

using Application.Common.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Common.Persistence;

internal abstract class BaseRepository<TEntity, TEntityId>
    : IBaseRepository<TEntity, TEntityId>
        where TEntity : AggregateRoot<TEntityId>
        where TEntityId : notnull
{
    private readonly IECommerceDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    /// <summary>
    /// Gets the database context used for data operations.
    /// </summary>
    protected IECommerceDbContext Context => _context;
    /// <summary>
    /// Gets the database set representing the aggregate collection.
    /// </summary>
    protected DbSet<TEntity> DbSet => _dbSet;

    protected BaseRepository(IECommerceDbContext dbContext)
    {
        _context = dbContext;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual async Task<IEnumerable<TEntity>> FindAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        CancellationToken cancellationToken = default
    )
    {

        if (filter != null)
        {
            return await _dbSet
                .Where(filter)
                .ToListAsync(cancellationToken);
        }

        return await _dbSet.ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity> FirstAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _dbSet.FirstAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> FindByIdAsync(
        TEntityId id,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbSet.FindAsync([id], cancellationToken);
    }

    public virtual async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbSet.FirstOrDefaultAsync(filter, cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> FindSatisfyingAsync(
        ISpecificationQuery<TEntity> specification,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbSet
            .Where(specification.Criteria)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> FindFirstSatisfyingAsync(
        ISpecificationQuery<TEntity> specification,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbSet
            .Where(specification.Criteria)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual void RemoveOrDeactivate(TEntity entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        if (entity is IActivatable activatable)
        {
            activatable.Deactivate();

            Context.Entry(entity).State = EntityState.Modified;
        }
        else
        {
            _dbSet.Remove(entity);
        }
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }
}
