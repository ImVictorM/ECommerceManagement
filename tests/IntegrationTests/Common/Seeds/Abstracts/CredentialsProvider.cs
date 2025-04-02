using IntegrationTests.Common.Seeds.DataTypes;

namespace IntegrationTests.Common.Seeds.Abstracts;

/// <summary>
/// Provides access to authentication credentials for integration tests.
/// </summary>
/// <typeparam name="TEnum">
/// The type of the enum used to identify seed data.
/// </typeparam>
public abstract class CredentialsProvider<TEnum> : ICredentialsProvider<TEnum>
    where TEnum : Enum
{
    private readonly Dictionary<TEnum, AuthenticationCredentials> _credentials;

    /// <summary>
    /// Initiates a new instance of the <see cref="CredentialsProvider{TEnum}"/>
    /// class.
    /// </summary>
    /// <param name="credentials">The credentials.</param>W
    protected CredentialsProvider(
        Dictionary<TEnum, AuthenticationCredentials> credentials
    )
    {
        _credentials = credentials;
    }

    /// <inheritdoc/>
    public AuthenticationCredentials GetCredentials(TEnum seedType)
    {
        if (_credentials.TryGetValue(seedType, out var result))
        {
            return result;
        }

        throw new InvalidOperationException(
            $"Credentials with type {nameof(seedType)} does not exist"
        );
    }

    /// <inheritdoc/>
    public string GetEmail(TEnum seedType)
    {
        return GetCredentials(seedType).Email;
    }

    /// <inheritdoc/>
    public string GetPassword(TEnum seedType)
    {
        return GetCredentials(seedType).Password;
    }

    /// <inheritdoc/>
    public IReadOnlyList<AuthenticationCredentials> ListCredentials()
    {
        return _credentials.Values.ToList();
    }
}
