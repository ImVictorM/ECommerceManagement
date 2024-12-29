using System.Linq.Expressions;
using SharedKernel.Models;

namespace SharedKernel.UnitTests.Models.TestUtils;

/// <summary>
/// Utilities for the <see cref="CompositeQuerySpecification{T}"/> class.
/// </summary>
public static class CompositeQuerySpecificationUtils
{
    private sealed class TestCompositeQuerySpecification<T> : CompositeQuerySpecification<T> where T : class
    {
        public TestCompositeQuerySpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public override Expression<Func<T, bool>> Criteria { get; }
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CompositeQuerySpecification{T}"/> class.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="expression">The expression.</param>
    /// <returns>A new instance of the <see cref="CompositeQuerySpecification{T}"/> class.</returns>
    public static CompositeQuerySpecification<T> CreateCompositeSpecification<T>(Expression<Func<T, bool>> expression) where T : class
    {
        return new TestCompositeQuerySpecification<T>(expression);
    }
}
