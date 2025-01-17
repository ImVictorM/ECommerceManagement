namespace Infrastructure.Security.Authentication.Settings;

/// <summary>
/// Maps the signature configuration options.
/// </summary>
public sealed class HmacSignatureSettings
{
    /// <summary>
    /// The section name in the app settings file.
    /// </summary>
    public const string SectionName = "HmacSignatureSettings";

    /// <summary>
    /// Gets the signature secret.
    /// </summary>
    public string Secret { get; init; } = null!;
}
