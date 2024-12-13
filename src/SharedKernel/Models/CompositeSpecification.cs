using SharedKernel.Interfaces;

namespace SharedKernel.Models;

/// <summary>
/// A composite specification that allows chaining multiple specifications using logical operators.
/// </summary>
/// <typeparam name="T">The type of entity this specification applies to.</typeparam>
public abstract class CompositeSpecification<T> : ISpecification<T> where T : class
{
    /// <summary>
    /// Combines the current specification with another using a logical AND operation.
    /// </summary>
    /// <param name="other">The other specification to combine.</param>
    /// <returns>A new composite specification representing the combined criteria.</returns>
    public CompositeSpecification<T> And(ISpecification<T> other)
    {
        return new AndSpecification<T>(this, other);
    }

    /// <inheritdoc/>
    public abstract bool IsSatisfiedBy(T entity);
}
