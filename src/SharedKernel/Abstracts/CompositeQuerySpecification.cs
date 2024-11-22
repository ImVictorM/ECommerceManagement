using System.Linq.Expressions;
using SharedKernel.Interfaces;

namespace SharedKernel.Abstracts;

/// <summary>
/// Composite query specification to create more complex criterias.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class CompositeQuerySpecification<T> : ISpecificationQuery<T> where T : class
{
    /// <inheritdoc/>
    public Expression<Func<T, bool>> Criteria { get; private set; }

    /// <summary>
    /// Initiates a new instance of the <see cref="CompositeQuerySpecification{T}"/> class.
    /// </summary>
    /// <param name="criteria">The specification criteria.</param>
    protected CompositeQuerySpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    /// <summary>
    /// Chains a specification criteria with the current criteria, creating a composite criteria based on the specifications chained.
    /// </summary>
    /// <param name="otherSpec">The other specification.</param>
    public void And(ISpecificationQuery<T> otherSpec)
    {
        var parameter = Expression.Parameter(typeof(T), "entity");

        var leftExpression = Expression.Invoke(Criteria, parameter);
        var rightExpression = Expression.Invoke(otherSpec.Criteria, parameter);

        var combinedExpression = Expression.AndAlso(leftExpression, rightExpression);

        Criteria = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
    }
}
