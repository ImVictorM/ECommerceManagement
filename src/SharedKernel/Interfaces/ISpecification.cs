namespace SharedKernel.Interfaces;

/// <summary>
/// Represents a specification pattern interface that defines a single criterion or rule
/// to be evaluated against an entity of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the entity that this specification applies to.</typeparam>
public interface ISpecification<in T> where T : class
{
    /// <summary>
    /// Determines whether the specified entity satisfies the current specification criteria.
    /// </summary>
    /// <param name="entity">The entity to evaluate against the specification.</param>
    /// <returns>A bool value indicating if the entity satisfies the specification.</returns>
    bool IsSatisfiedBy(T entity);
}
