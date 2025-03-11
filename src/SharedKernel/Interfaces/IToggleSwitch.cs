namespace SharedKernel.Interfaces;

/// <summary>
/// Defines a contract for objects that can be toggled between two states,
/// such as active and inactive.
/// </summary>
public interface IToggleSwitch
{
    /// <summary>
    /// Gets a value indicating whether the object is currently active.
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// Toggles the active state of the object.
    /// </summary>
    void ToggleActivation();
}
