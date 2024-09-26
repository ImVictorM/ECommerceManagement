namespace Infrastructure.Persistence;

/// <summary>
/// Maps the configuration for the database connection.
/// </summary>
public class ECommerceDatabaseConnectionSettings
{
    /// <summary>
    /// The appsettings.json section name that it maps.
    /// </summary>
    public const string SectionName = "ConnectionStrings";

    /// <summary>
    /// The connection string to be used.
    /// </summary>
    public string DefaultConnection { get; init; } = null!;
}
