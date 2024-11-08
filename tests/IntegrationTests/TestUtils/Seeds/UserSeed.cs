using Domain.Common.ValueObjects;
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
    /// Other admin with different name and email.
    /// </summary>
    OtherAdmin,
    /// <summary>
    /// A customer user.
    /// </summary>
    User,
    /// <summary>
    /// A customer user with an address.
    /// </summary>
    UserWithAddress,
    /// <summary>
    /// A customer inactive user.
    /// </summary>
    InactiveUser,
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

        [SeedAvailableUsers.OtherAdmin] = (UserUtils.CreateUser(
            name: "other admin",
            role: Role.Admin,
            email: "otherm_admin@email.com",
            passwordHash: "6333824CC074E187E261A0CBBD91F9741B4D38A26E1519A93B4244BEAFC933B9",
            passwordSalt: "4FDE231393F2C8AECC2B26F356E3D89E"
        ), AdminPassword),

        [SeedAvailableUsers.User] = (UserUtils.CreateUser(
            name: "normal user",
            email: "user_normal@email.com",
            role: Role.Customer
        ), UserPassword),

        [SeedAvailableUsers.UserWithAddress] = (UserUtils.CreateUser(
            name: "user with address",
            email: "user_addr@email.com",
            role: Role.Customer,
            addresses: new List<Address> { AddressUtils.CreateAddress() }.AsReadOnly()
        ), UserPassword),

        [SeedAvailableUsers.InactiveUser] = (UserUtils.CreateUser(
            name: "inactive user",
            email: "user_inactive@email.com",
            role: Role.Customer,
            active: false
        ), UserPassword),
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
    /// <param name="filter">Filter the users based on a predicate.</param>
    /// <returns>A list of all seed users.</returns>
    public static IEnumerable<User> ListUsers(Func<User, bool>? filter = null)
    {
        var users = _users.Values.Select(u => u.User);

        return filter != null ? users.Where(filter) : users;
    }

    /// <summary>
    /// List all the credentials for the seed users. By default retrieves only active users.
    /// </summary>
    /// <param name="filter">Filter the users credentials based on a predicate.</param>
    /// <returns>A tuple of email and password for each seed user.</returns>
    public static IEnumerable<(string Email, string Password)> ListUsersCredentials(Func<User, bool>? filter = null)
    {

        return filter != null ?
            _users.Values.Where(u => filter(u.User)).Select(u => (u.User.Email.ToString(), u.Password)) :
            _users.Values.Select(u => (u.User.Email.ToString(), u.Password));
    }
}
