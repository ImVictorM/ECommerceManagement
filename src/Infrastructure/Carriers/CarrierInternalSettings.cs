namespace Infrastructure.Carriers;

/// <summary>
/// Represents the internal carrier configuration from appsettings.
/// </summary>
public class CarrierInternalSettings
{
    /// <summary>
    /// The appsettings section name.
    /// </summary>
    public const string SectionName = "CarrierInternal";
    /// <summary>
    /// Gets the carrier name;
    /// </summary>
    public string Name { get; init; } = null!;
    /// <summary>
    /// Gets the carrier email address.
    /// </summary>
    public string Email { get; init; } = null!;
    /// <summary>
    /// Gets the carrier password.
    /// </summary>
    public string Password { get; init; } = null!;
    /// <summary>
    /// Gets the carrier phone.
    /// </summary>
    public string? Phone { get; init; }
}
