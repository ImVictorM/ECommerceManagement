using Application.Common.Security.Authorization.Roles;

using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using Infrastructure.Security.Authentication;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

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
    Customer,
    /// <summary>
    /// A customer user with an address.
    /// </summary>
    CustomerWithAddress,
    /// <summary>
    /// A customer inactive user.
    /// </summary>
    InactiveCustomer,
}

/// <summary>
/// UserSeed class containing seed data for users and their credentials.
/// </summary>
public static class UserSeed
{
    private const string AdminPassword = "admin123";
    private const string UserPassword = "user123";
    private static readonly PasswordHasher _hasher = new PasswordHasher();

    private static readonly Dictionary<SeedAvailableUsers, (User User, string Password)> _users = new()
    {
        [SeedAvailableUsers.Admin] = (UserUtils.CreateUser(
            id: UserId.Create(-1),
            name: "admin",
            roles: new HashSet<UserRole>() { UserRole.Create(Role.Admin.Id) },
            email: EmailUtils.CreateEmail("system_admin@email.com"),
            passwordHash: _hasher.Hash(AdminPassword)
        ), AdminPassword),

        [SeedAvailableUsers.OtherAdmin] = (UserUtils.CreateUser(
            id: UserId.Create(-2),
            name: "other admin",
            roles: new HashSet<UserRole>() { UserRole.Create(Role.Admin.Id) },
            email: EmailUtils.CreateEmail("other_admin@email.com"),
            passwordHash: _hasher.Hash(AdminPassword)
        ), AdminPassword),

        [SeedAvailableUsers.Customer] = (UserUtils.CreateUser(
            id: UserId.Create(-3),
            name: "normal user",
            email: EmailUtils.CreateEmail("user_normal@email.com"),
            roles: new HashSet<UserRole>() { UserRole.Create(Role.Customer.Id) },
            passwordHash: _hasher.Hash(UserPassword)
        ), UserPassword),

        [SeedAvailableUsers.CustomerWithAddress] = (UserUtils.CreateUser(
            id: UserId.Create(-4),
            name: "user with address",
            email: EmailUtils.CreateEmail("user_address@email.com"),
            roles: new HashSet<UserRole>() { UserRole.Create(Role.Customer.Id) },
            addresses: new HashSet<Address>() { AddressUtils.CreateAddress() },
            passwordHash: _hasher.Hash(UserPassword)
        ), UserPassword),

        [SeedAvailableUsers.InactiveCustomer] = (UserUtils.CreateUser(
            id: UserId.Create(-5),
            name: "inactive user",
            email: EmailUtils.CreateEmail("user_inactive@email.com"),
            roles: new HashSet<UserRole>() { UserRole.Create(Role.Customer.Id) },
            passwordHash: _hasher.Hash(UserPassword),
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
    public static IReadOnlyList<User> ListUsers(Func<User, bool>? filter = null)
    {
        var users = _users.Values.Select(u => u.User);

        return filter != null ? users.Where(filter).ToList() : [.. users];
    }

    /// <summary>
    /// List all the credentials for the seed users. By default retrieves only active users.
    /// </summary>
    /// <param name="filter">Filter the users credentials based on a predicate.</param>
    /// <returns>A tuple of email and password for each seed user.</returns>
    public static IReadOnlyList<(string Email, string Password)> ListUsersCredentials(Func<User, bool>? filter = null)
    {

        return filter != null ?
            _users.Values.Where(u => filter(u.User)).Select(u => (u.User.Email.ToString(), u.Password)).ToList() :
            _users.Values.Select(u => (u.User.Email.ToString(), u.Password)).ToList();
    }
}
