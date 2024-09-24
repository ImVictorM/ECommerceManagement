namespace Domain.Common.Interfaces;

/// <summary>
/// Define properties to track an object.
/// </summary>
public interface ITrackable
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
