using SharedKernel.Interfaces;

namespace SharedKernel.UnitTests.Models.TestUtils;

/// <summary>
/// Utilities for the <see cref="ISpecification{T}"/> class.
/// </summary>
public static class SpecificationUtils
{
    private sealed class TestSpecification<T> : ISpecification<T> where T : class
    {
        private readonly Func<T, bool> _predicate;

        public TestSpecification(Func<T, bool> predicate)
        {
            _predicate = predicate;
        }

        public bool IsSatisfiedBy(T entity)
        {
            return _predicate(entity);
        }
    }

    /// <summary>
    /// Creates a test specification based on a provided predicate.
    /// </summary>
    /// <typeparam name="T">
    /// The type of entity the specification applies to.
    /// </typeparam>
    /// <param name="predicate">
    /// The predicate to evaluate for the specification.
    /// </param>
    /// <returns>An implementation of <see cref="ISpecification{T}"/>.</returns>
    public static ISpecification<T> CreateSpecification<T>(
        Func<T, bool> predicate
    ) where T : class
    {
        return new TestSpecification<T>(predicate);
    }
}
