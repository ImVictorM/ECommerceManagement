using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using SharedKernel.Authorization;

namespace IntegrationTests.TestUtils.Seeds;

/// <summary>
/// Maps the available users in the seed.
/// </summary>
public enum SeedAvailableUsers
{
    /// <summary>
    /// The admin user.
    /// </summary>
    Admin,
    /// <summary>
    /// A normal user.
    /// </summary>
    User1,
    /// <summary>
    /// A normal user.
    /// </summary>
    User2
}

/// <summary>
/// UserSeed class containing seed data for users and their credentials.
/// </summary>
public static class UserSeed
{
    private const string AdminPassword = "admin123";
    private const string UserPassword = "user123";

    private static readonly Dictionary<SeedAvailableUsers, (User User, string Password)> _users = new()
    {
        [SeedAvailableUsers.Admin] = (UserUtils.CreateUser(
            name: "admin",
            role: Role.Admin,
            email: "system_admin@email.com",
            passwordHash: "6333824CC074E187E261A0CBBD91F9741B4D38A26E1519A93B4244BEAFC933B9",
            passwordSalt: "4FDE231393F2C8AECC2B26F356E3D89E"
        ), AdminPassword),

        [SeedAvailableUsers.User1] = (UserUtils.CreateUser(
            name: "user 1",
            email: "user1@email.com",
            role: Role.Customer
        ), UserPassword),

        [SeedAvailableUsers.User2] = (UserUtils.CreateUser(
            name: "user 2",
            email: "user2@email.com",
            role: Role.Customer
        ), UserPassword)
    };

    /// <summary>
    /// Get the authentication credentials for a seed user.
    /// </summary>
    /// <param name="seedUser">The user to get the credentials from.</param>
    /// <returns>A tuple containing the email and password.</returns>
    public static (string Email, string Password) GetUserAuthenticationCredentials(SeedAvailableUsers seedUser)
    {
        var user = _users[seedUser];

        return (user.User.Email.ToString(), user.Password);
    }

    /// <summary>
    /// Gets a seed user.
    /// </summary>
    /// <param name="seedUser">The user type.</param>
    /// <returns>The user.</returns>
    public static User GetSeedUser(SeedAvailableUsers seedUser)
    {
        return _users[seedUser].User;
    }

    /// <summary>
    /// List all seed users.
    /// </summary>
    /// <returns>A list of all seed users.</returns>
    public static IEnumerable<User> ListUsers(Func<User, bool>? filter = null)
    {
        var users = _users.Values.Select(u => u.User);

        return filter != null ? users.Where(filter) : users;
    }

    /// <summary>
    /// List all the credentials for the seed users.
    /// </summary>
    /// <returns>A tuple of email and password for each seed user.</returns>
    public static IEnumerable<(string Email, string Password)> ListUsersCredentials()
    {
        return _users.Values.Select(u => (u.User.Email.ToString(), u.Password));
    }
}
