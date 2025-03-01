using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.Users;

/// <summary>
/// Defines a contract to provide access to authentication credentials for users.
/// </summary>
public interface IUserCredentialsProvider : ICredentialsProvider<UserSeedType>
{
}
