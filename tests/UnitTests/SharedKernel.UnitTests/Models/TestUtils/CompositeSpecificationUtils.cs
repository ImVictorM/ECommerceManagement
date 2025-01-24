using SharedKernel.Models;

namespace SharedKernel.UnitTests.Models.TestUtils;

/// <summary>
/// Utilities for the <see cref="CompositeSpecification{T}"/> class.
/// </summary>
public static class CompositeSpecificationUtils
{
    /// <summary>
    /// A concrete implementation of <see cref="CompositeSpecification{T}"/> for testing.
    /// </summary>
    public class TestCompositeSpecification<T> : CompositeSpecification<T> where T : class
    {
        private readonly Func<T, bool> _predicate;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestCompositeSpecification{T}"/> class.
        /// </summary>
        /// <param name="predicate">The predicate to evaluate.</param>
        public TestCompositeSpecification(Func<T, bool> predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc/>
        public override bool IsSatisfiedBy(T entity)
        {
            return _predicate(entity);
        }
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CompositeSpecification{T}"/> class.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="predicate">The predicate.</param>
    /// <returns>A new instance of the <see cref="CompositeSpecification{T}"/> class.</returns>
    public static CompositeSpecification<T> CreateCompositeSpecification<T>(Func<T, bool> predicate) where T : class
    {
        return new TestCompositeSpecification<T>(predicate);
    }
}
