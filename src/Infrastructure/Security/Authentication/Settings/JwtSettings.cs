namespace Infrastructure.Security.Authentication.Settings;

/// <summary>
/// JWT Token settings.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// The settings section name.
    /// </summary>
    public const string SectionName = "JwtSettings";
    /// <summary>
    /// Gets token secret.
    /// </summary>
    public string Secret { get; init; } = null!;
    /// <summary>
    /// Gets token issuer.
    /// </summary>
    public string Issuer { get; init; } = null!;
    /// <summary>
    /// Gets token expiration in minutes.
    /// </summary>
    public int ExpiresInMinutes { get; init; }
    /// <summary>
    /// Gets token audience.
    /// </summary>
    public string Audience { get; init; } = null!;
}
