using SharedKernel.Models;

namespace SharedKernel.UnitTests.Models.TestUtils;

/// <summary>
/// AggregateRoot utilities.
/// </summary>
public static class AggregateRootUtils
{
    /// <summary>
    /// Create instances of the generic AggregateRoot.
    /// </summary>
    public class UserAggregateRoot<TId> : AggregateRoot<TId> where TId : notnull
    {
        /// <summary>
        /// Initiates a new instance of the <see cref="UserAggregateRoot{TId}"/> class.
        /// </summary>
        /// <param name="id">The entity unique identifier.</param>
        public UserAggregateRoot(TId id) : base(id)
        {

        }
    }

    /// <summary>
    /// Create instances of the generic UserAggregateRoot.
    /// </summary>
    public static class UserAggregateRoot
    {
        /// <summary>
        /// Creates a new instance of the <see cref="UserAggregateRoot{TId}"/> class.
        /// </summary>
        /// <typeparam name="T">The type of the entity ID.</typeparam>
        /// <param name="id">The entity unique identifier.</param>
        /// <returns>A new instance of the <see cref="UserAggregateRoot{TId}"/> class.</returns>
        public static UserAggregateRoot<T> Create<T>(T id) where T : notnull
        {
            return new UserAggregateRoot<T>(id);
        }
    }
}
