using SharedKernel.Interfaces;

namespace SharedKernel.Models;

/// <summary>
/// Specification that combines two specification using the AND operator.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public class AndSpecification<T> : CompositeSpecification<T> where T : class
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    /// <summary>
    /// Initiates a new instance other <see cref="AndSpecification{T}"/> class.
    /// </summary>
    /// <param name="left">The left specification.</param>
    /// <param name="right">The right specification.</param>
    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    /// <inheritdoc/>
    public override bool IsSatisfiedBy(T entity)
    {
        return _left.IsSatisfiedBy(entity) && _right.IsSatisfiedBy(entity);
    }
}
