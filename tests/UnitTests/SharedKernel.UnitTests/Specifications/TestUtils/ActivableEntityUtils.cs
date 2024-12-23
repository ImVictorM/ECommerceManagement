using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace SharedKernel.UnitTests.Specifications.TestUtils;

/// <summary>
/// Utilities to create activable entities.
/// </summary>
public static class ActivableEntityUtils
{
    /// <summary>
    /// Represents an entity that can be activated or deactivated.
    /// </summary>
    public class ActivableEntity : Entity<Guid>, IActivatable
    {
        /// <summary>
        /// Bool value indicating if the entity is active.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Initiates a new instance of the <see cref="ActivableEntity"/> class.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <param name="isActive">The entity state.</param>
        public ActivableEntity(Guid id, bool isActive) : base(id)
        {
            IsActive = isActive;
        }

        ///<inheritdoc/>
        public void Deactivate()
        {
            IsActive = false;
        }
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ActivableEntity"/> class.
    /// </summary>
    /// <param name="id">The entity id.</param>
    /// <param name="isActive">The entity state.</param>
    /// <returns>A new instance of the <see cref="ActivableEntity"/> class.</returns>
    public static ActivableEntity CreateActivableEntity(
        Guid? id = null,
        bool isActive = true
    )
    {
        return new ActivableEntity(id ?? Guid.NewGuid(), isActive);
    }
}
