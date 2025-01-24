using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace SharedKernel.UnitTests.Specifications.TestUtils;

/// <summary>
/// Utilities to create activatable entities.
/// </summary>
public static class ActivatableEntityUtils
{
    /// <summary>
    /// Represents an entity that can be activated or deactivated.
    /// </summary>
    public class ActivatableEntity : Entity<Guid>, IActivatable
    {
        /// <summary>
        /// Bool value indicating if the entity is active.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Initiates a new instance of the <see cref="ActivatableEntity"/> class.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <param name="isActive">The entity state.</param>
        public ActivatableEntity(Guid id, bool isActive) : base(id)
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
    /// Creates a new instance of the <see cref="ActivatableEntity"/> class.
    /// </summary>
    /// <param name="id">The entity id.</param>
    /// <param name="isActive">The entity state.</param>
    /// <returns>A new instance of the <see cref="ActivatableEntity"/> class.</returns>
    public static ActivatableEntity CreateActivatableEntity(
        Guid? id = null,
        bool isActive = true
    )
    {
        return new ActivatableEntity(id ?? Guid.NewGuid(), isActive);
    }
}
