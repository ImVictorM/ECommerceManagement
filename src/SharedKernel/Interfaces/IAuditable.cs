namespace SharedKernel.Interfaces;

/// <summary>
/// Contract to define properties to track an object.
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// Gets the date when the object was created.
    /// </summary>
    DateTimeOffset CreatedAt { get; }

    /// <summary>
    /// Gets the date when the object was last updated.
    /// </summary>
    DateTimeOffset UpdatedAt { get; }
}
