using System.Linq.Expressions;
using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace SharedKernel.Specifications;

/// <summary>
/// Query specification to retrieve an active object.
/// </summary>
public class QueryActiveSpecification<T> : CompositeQuerySpecification<T> where T : class, IActivatable
{
    /// <inheritdoc/>
    public override Expression<Func<T, bool>> Criteria => entity => entity.IsActive;
}
