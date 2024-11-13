namespace SharedKernel.Interfaces;

/// <summary>
/// Defines a contract for entities that can ne soft deleted.
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// Gets a boolean value indicating if the entity is active.
    /// </summary>
    public bool IsActive { get; }

    /// <summary>
    /// Makes the entity inactive.
    /// </summary>
    public void MakeInactive();
}
