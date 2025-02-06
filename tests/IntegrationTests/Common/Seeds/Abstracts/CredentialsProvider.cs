using IntegrationTests.Common.Seeds.DataTypes;

namespace IntegrationTests.Common.Seeds.Abstracts;

/// <summary>
/// Provides access to authentication credentials for integration tests.
/// </summary>
/// <typeparam name="TEnum">The type of the enum used to identify seed data.</typeparam>
public abstract class CredentialsProvider<TEnum> : ICredentialsProvider<TEnum>
    where TEnum : Enum
{
    private readonly Dictionary<TEnum, AuthenticationCredentials> _credentials;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="credentials"></param>
    protected CredentialsProvider(Dictionary<TEnum, AuthenticationCredentials> credentials)
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

        throw new InvalidOperationException($"Credentials with type {nameof(seedType)} does not exist");
    }

    /// <inheritdoc/>
    public IReadOnlyList<AuthenticationCredentials> ListCredentials()
    {
        return _credentials.Values.ToList();
    }
}
