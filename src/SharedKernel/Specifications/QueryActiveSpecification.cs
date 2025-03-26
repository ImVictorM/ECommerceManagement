using SharedKernel.Interfaces;
using SharedKernel.Models;

using System.Linq.Expressions;

namespace SharedKernel.Specifications;

/// <summary>
/// Query specification to retrieve active entities.
/// </summary>
public class QueryActiveSpecification<T>
    : CompositeQuerySpecification<T> where T : class, IActivatable
{
    /// <inheritdoc/>
    public override Expression<Func<T, bool>> Criteria => entity => entity.IsActive;
}
