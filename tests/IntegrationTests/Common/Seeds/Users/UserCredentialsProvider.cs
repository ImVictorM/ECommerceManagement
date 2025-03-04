using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.DataTypes;

namespace IntegrationTests.Common.Seeds.Users;

/// <summary>
/// Represents a credentials provider for seed users.
/// </summary>
public class UserCredentialsProvider
    : CredentialsProvider<UserSeedType>, IUserCredentialsProvider
{
    private static Dictionary<UserSeedType, AuthenticationCredentials> credentials => new()
    {
        [UserSeedType.ADMIN] = new AuthenticationCredentials(
            "system_admin@email.com",
            "admin123"
        ),
        [UserSeedType.OTHER_ADMIN] = new AuthenticationCredentials(
            "other_admin@email.com",
            "other-admin123"
        ),
        [UserSeedType.CUSTOMER] = new AuthenticationCredentials(
            "user_normal@email.com",
            "customer123"
        ),
        [UserSeedType.CUSTOMER_WITH_ADDRESS] = new AuthenticationCredentials(
            "user_address@email.com",
            "customer-with-address-123"
        ),
        [UserSeedType.CUSTOMER_INACTIVE] = new AuthenticationCredentials(
            "user_inactive@email.com",
            "customer-inactive-123"
        )
    };

    /// <summary>
    /// Initiates a new instance of the <see cref="UserCredentialsProvider"/> class.
    /// </summary>
    public UserCredentialsProvider() : base(credentials)
    {
    }
}
