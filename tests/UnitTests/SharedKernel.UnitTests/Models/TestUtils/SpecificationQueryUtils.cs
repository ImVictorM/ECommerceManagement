using SharedKernel.Interfaces;

using System.Linq.Expressions;

namespace SharedKernel.UnitTests.Models.TestUtils;

/// <summary>
/// Utilities for the <see cref="ISpecificationQuery{T}"/> class.
/// </summary>
public static class SpecificationQueryUtils
{
    private sealed class TestSpecificationQuery<T>
        : ISpecificationQuery<T> where T : class
    {
        public TestSpecificationQuery(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }
    }

    /// <summary>
    /// Creates a test specification query based on a provided expression.
    /// </summary>
    /// <typeparam name="T">
    /// The type of entity the specification applies to.
    /// </typeparam>
    /// <param name="expression">
    /// The expression to evaluate for the specification.
    /// </param>
    /// <returns>
    /// An implementation of <see cref="ISpecificationQuery{T}"/>.
    /// </returns>
    public static ISpecificationQuery<T> CreateSpecificationQuery<T>(
        Expression<Func<T, bool>> expression
    ) where T : class
    {
        return new TestSpecificationQuery<T>(expression);
    }
}
