using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace SharedKernel.Specifications;

/// <summary>
/// Query specification to retrieve an entity that is active.
/// </summary>
public class QueryActiveEntityByIdSpecification<TEntity, TEntityId> : CompositeQuerySpecification<TEntity>
    where TEntity : Entity<TEntityId>, ISoftDeletable
    where TEntityId : notnull
{
    /// <summary>
    /// Initiates a new instance of the <see cref="QueryActiveEntityByIdSpecification{TEntity, TEntityId}"/> class.
    /// </summary>
    public QueryActiveEntityByIdSpecification(TEntityId id) : base(entity => entity.IsActive && entity.Id.Equals(id))
    {
    }
}
