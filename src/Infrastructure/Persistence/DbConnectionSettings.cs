namespace Infrastructure.Persistence;

/// <summary>
/// Maps the configuration for the database connection.
/// </summary>
public class DbConnectionSettings
{
    /// <summary>
    /// The appsettings.json section name that it maps.
    /// </summary>
    public const string SectionName = "DbConnectionSettings";

    /// <summary>
    /// The connection host.
    /// </summary>
    public string Host { get; init; } = null!;

    /// <summary>
    /// The connection port.
    /// </summary>
    public string Port { get; init; } = null!;

    /// <summary>
    /// The connection database name.
    /// </summary>
    public string Database { get; init; } = null!;

    /// <summary>
    /// The connection username.
    /// </summary>
    public string Username { get; init; } = null!;

    /// <summary>
    /// The connection password.
    /// </summary>
    public string Password { get; init; } = null!;
}
