using IntegrationTests.Common.Seeds.DataTypes;

namespace IntegrationTests.Common.Seeds.Abstracts;

/// <summary>
/// Provides access to authentication credentials for integration tests.
/// </summary>
/// <typeparam name="TEnum">The type of the enum used to identify seed data.</typeparam>
public interface ICredentialsProvider<TEnum>
    where TEnum : Enum
{
    /// <summary>
    /// Retrieves the authentication credentials for a seed entity.
    /// </summary>
    /// <param name="seedType">The entity type to get the credentials for.</param>
    /// <returns>The authentication credentials containing email and password.</returns>
    AuthenticationCredentials GetCredentials(TEnum seedType);

    /// <summary>
    /// Retrieves all the seed entity credentials.
    /// </summary>
    /// <returns>A list containing the authentication credentials for each seed entity.</returns>
    IReadOnlyList<AuthenticationCredentials> ListCredentials();
}
