using SharedKernel.Interfaces;

using System.Linq.Expressions;

namespace SharedKernel.Models;

/// <summary>
/// Composite query specification to create more complex criteria.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public abstract class CompositeQuerySpecification<T>
    : ISpecificationQuery<T> where T : class
{
    /// <inheritdoc/>
    public abstract Expression<Func<T, bool>> Criteria { get; }

    /// <summary>
    /// Chains a specification criteria with the current criteria,
    /// creating a composite criteria based on the specifications chained.
    /// </summary>
    /// <param name="otherSpec">The other specification.</param>
    public CompositeQuerySpecification<T> And(ISpecificationQuery<T> otherSpec)
    {
        return new AndQuerySpecification<T>(this, otherSpec);
    }
}
