using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace SharedKernel.Specifications;

/// <summary>
/// Query specification to retrieve an active object.
/// </summary>
public class QueryActiveSpecification<T> : CompositeQuerySpecification<T> where T : class, ISoftDeletable
{
    /// <summary>
    /// Initiates a new instance of the <see cref="QueryActiveSpecification{T}"/> class.
    /// </summary>
    public QueryActiveSpecification() : base(entity => entity.IsActive)
    {
    }
}
