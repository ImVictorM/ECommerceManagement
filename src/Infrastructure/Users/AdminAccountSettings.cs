namespace Infrastructure.Users;

/// <summary>
/// Represents the admin account configuration from appsettings.
/// </summary>
public class AdminAccountSettings
{
    /// <summary>
    /// The appsettings section name.
    /// </summary>
    public const string SectionName = "AdminAccount";
    /// <summary>
    /// Gets the admin name.
    /// </summary>
    public string Name { get; init; } = null!;
    /// <summary>
    /// Gets the admin email address.
    /// </summary>
    public string Email { get; init; } = null!;
    /// <summary>
    /// Gets the admin password.
    /// </summary>
    public string Password { get; init; } = null!;
}
