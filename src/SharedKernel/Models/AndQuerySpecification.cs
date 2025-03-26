using SharedKernel.Interfaces;

using System.Linq.Expressions;

namespace SharedKernel.Models;

/// <summary>
/// A composite query specification that allows chaining multiple specifications.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public class AndQuerySpecification<T>
    : CompositeQuerySpecification<T> where T : class
{
    private readonly ISpecificationQuery<T> _left;
    private readonly ISpecificationQuery<T> _right;

    /// <summary>
    /// Initiates a new instance of the <see cref="AndQuerySpecification{T}"/>
    /// class.
    /// </summary>
    /// <param name="left">The left specification.</param>
    /// <param name="right">The right specification.</param>
    public AndQuerySpecification(
        ISpecificationQuery<T> left,
        ISpecificationQuery<T> right
    )
    {
        _left = left;
        _right = right;
    }

    /// <inheritdoc/>
    public override Expression<Func<T, bool>> Criteria
    {
        get
        {
            var parameter = Expression.Parameter(typeof(T), "entity");

            var combinedExpression = Expression.AndAlso(
                Expression.Invoke(_left.Criteria, parameter),
                Expression.Invoke(_right.Criteria, parameter)
            );

            return Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
        }
    }
}
