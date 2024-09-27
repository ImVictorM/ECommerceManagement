using System.Linq.Expressions;
using Domain.Common.Models;

namespace Application.Common.Interfaces.Persistence;

/// <summary>
/// Generic repository to handle aggregates persistence.
/// </summary>
/// <typeparam name="TEntity">The aggregate type.</typeparam>
/// <typeparam name="TEntityId">The aggregate id type.</typeparam>
public interface IRepository<TEntity, TEntityId>
    where TEntity : AggregateRoot<TEntityId>
    where TEntityId : notnull
{
    /// <summary>
    /// Find all records.
    /// </summary>
    /// <param name="filter">A nullable filter expression.</param>
    /// <returns>An iterable of type <typeparamref name="TEntity"/>.</returns>
    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>>? filter = null);

    /// <summary>
    /// Gets the first record that matches the filter.
    /// </summary>
    /// <param name="filter">The expression to filter the records.</param>
    /// <returns>The first record that matches the filter condition.</returns>
    Task<TEntity?> FindOneOrDefaultAsync(Expression<Func<TEntity, bool>> filter);

    /// <summary>
    /// Find a record by its identifier.
    /// </summary>
    /// <param name="id">The identifier of type <typeparamref name="TEntityId"/>.</param>
    /// <returns>The record that matches the identifier.</returns>
    Task<TEntity?> FindByIdAsync(TEntityId id);

    /// <summary>
    /// Adds a new record.
    /// </summary>
    /// <param name="entity">The entity to be registered.</param>
    /// <returns>An asynchronous operation.</returns>
    Task AddAsync(TEntity entity);

    /// <summary>
    /// Removes or inactives a record.
    /// </summary>
    /// <param name="id">The record identifier.</param>
    /// <returns>An asynchronous operation.</returns>
    Task RemoveAsync(TEntityId id);

    /// <summary>
    /// Updates an existing record.
    /// </summary>
    /// <param name="entity">The updated entity.</param>
    /// <returns>An asynchronous operation.</returns>
    Task UpdateAsync(TEntity entity);
}
