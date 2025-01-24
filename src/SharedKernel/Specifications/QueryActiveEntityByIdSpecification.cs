using System.Linq.Expressions;
using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace SharedKernel.Specifications;

/// <summary>
/// Query specification to retrieve an entity that is active by id.
/// </summary>
public class QueryActiveEntityByIdSpecification<TEntity, TEntityId> : CompositeQuerySpecification<TEntity>
    where TEntity : Entity<TEntityId>, IActivatable
    where TEntityId : notnull
{
    private readonly TEntityId _id;

    /// <summary>
    /// Initiates a new instance of the <see cref="QueryActiveEntityByIdSpecification{TEntity, TEntityId}"/> class.
    /// </summary>
    /// <param name="id">The entity id.</param>
    public QueryActiveEntityByIdSpecification(TEntityId id)
    {
        _id = id;
    }

    /// <inheritdoc/>
    public override Expression<Func<TEntity, bool>> Criteria => entity => entity.IsActive && entity.Id.Equals(_id);
}
