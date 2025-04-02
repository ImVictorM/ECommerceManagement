using System.Linq.Expressions;

namespace SharedKernel.Interfaces;

/// <summary>
/// Represents a specification pattern interface that defines a single predicate
/// to be evaluated against an entity of type <typeparamref name="T"/>.
/// This interface is useful specially when creating predicates for database queries,
/// preventing to have data in memory.
/// </summary>
/// <typeparam name="T">
/// The type of the entity that this specification applies to.
/// </typeparam>
public interface ISpecificationQuery<T> where T : class
{
    /// <summary>
    /// Gets the specification criteria.
    /// </summary>
    Expression<Func<T, bool>> Criteria { get; }
}
