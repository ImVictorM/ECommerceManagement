using SharedKernel.Interfaces;
using SharedKernel.Models;

using System.Linq.Expressions;

namespace Application.Common.Persistence.Repositories;

/// <summary>
/// Represents a base repository to handle aggregate operations.
/// </summary>
/// <typeparam name="TEntity">The aggregate type.</typeparam>
/// <typeparam name="TEntityId">The aggregate id type.</typeparam>
public interface IBaseRepository<TEntity, TEntityId>
    where TEntity : AggregateRoot<TEntityId>
    where TEntityId : notnull
{
    /// <summary>
    /// Retrieves the first <typeparamref name="TEntity"/> element.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The first <typeparamref name="TEntity"/> element.</returns>
    Task<TEntity> FirstAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the first element that matches the filter or default.
    /// </summary>
    /// <param name="filter">The expression to filter the elements.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The first element that matches the filter condition.</returns>
    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves all <typeparamref name="TEntity"/> elements.
    /// </summary>
    /// <param name="filter">A filter expression.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An iterator of type <typeparamref name="TEntity"/>.</returns>
    Task<IEnumerable<TEntity>> FindAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves an element with the specified <typeparamref name="TEntityId"/> identifier.
    /// </summary>
    /// <param name="id">The identifier of type <typeparamref name="TEntityId"/>.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The record that matches the identifier.</returns>
    Task<TEntity?> FindByIdAsync(
        TEntityId id,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves all elements satisfying an specification.
    /// </summary>
    /// <param name="specification">The specification.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>All elements satisfying the specification.</returns>
    Task<IEnumerable<TEntity>> FindSatisfyingAsync(
        ISpecificationQuery<TEntity> specification,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves the first element satisfying an specification.
    /// </summary>
    /// <param name="specification">The specification.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The first element that satisfies the specification criteria.</returns>
    Task<TEntity?> FindFirstSatisfyingAsync(
        ISpecificationQuery<TEntity> specification,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Adds a new record.
    /// </summary>
    /// <param name="entity">The entity to be registered.</param>
    Task AddAsync(TEntity entity);

    /// <summary>
    /// Marks an entity as removed or Deactivates an <see cref="IActivatable"/> entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    void RemoveOrDeactivate(TEntity entity);

    /// <summary>
    /// Marks and entity as updated.
    /// </summary>
    /// <param name="entity">The updated entity.</param>
    void Update(TEntity entity);
}
