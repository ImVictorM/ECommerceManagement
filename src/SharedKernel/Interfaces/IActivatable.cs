namespace SharedKernel.Interfaces;

/// <summary>
/// Defines a contract for objects that has an active status.
/// </summary>
public interface IActivatable
{
    /// <summary>
    /// Gets a value indicating whether the object is currently active.
    /// </summary>
    public bool IsActive { get; }

    /// <summary>
    /// Deactivates the object, marking it as inactive.
    /// </summary>
    public void Deactivate();
}
