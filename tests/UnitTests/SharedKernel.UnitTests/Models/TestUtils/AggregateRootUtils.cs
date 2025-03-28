using SharedKernel.Models;

namespace SharedKernel.UnitTests.Models.TestUtils;

/// <summary>
/// Utilities for the <see cref="AggregateRoot{TId}"/> model.
/// </summary>
public static class AggregateRootUtils
{
    /// <summary>
    /// Represents a user aggregate root.
    /// </summary>
    public class UserAggregateRoot<TId> : AggregateRoot<TId> where TId : notnull
    {
        /// <summary>
        /// Initiates a new instance of the <see cref="UserAggregateRoot{TId}"/>
        /// class.
        /// </summary>
        /// <param name="id">The entity unique identifier.</param>
        public UserAggregateRoot(TId id) : base(id)
        {
        }
    }

    /// <summary>
    /// Defines a factory to create instance of the
    /// <see cref="UserAggregateRoot{TId}"/> class.
    /// </summary>
    public static class UserAggregateRoot
    {
        /// <summary>
        /// Creates a new instance of the <see cref="UserAggregateRoot{TId}"/>
        /// class.
        /// </summary>
        /// <typeparam name="T">The type of the entity identifier.</typeparam>
        /// <param name="id">The entity unique identifier.</param>
        /// <returns>
        /// A new instance of the <see cref="UserAggregateRoot{TId}"/> class.
        /// </returns>
        public static UserAggregateRoot<T> Create<T>(T id) where T : notnull
        {
            return new UserAggregateRoot<T>(id);
        }
    }
}
