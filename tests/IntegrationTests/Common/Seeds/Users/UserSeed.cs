using Application.Common.Security.Authorization.Roles;

using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

using Infrastructure.Security.Authentication;
using Infrastructure.Common.Persistence;

using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.Users;

/// <summary>
/// Provides seed data for users in the database.
/// </summary>
public sealed class UserSeed : DataSeed<UserSeedType, User>
{
    private static readonly PasswordHasher _hasher = new();

    private static Dictionary<UserSeedType, (User User, string Password)> _usersWithCredentials => new()
    {
        [UserSeedType.ADMIN] = (
            UserUtils.CreateUser
            (
                id: UserId.Create(-1),
                name: "admin",
                roles: new HashSet<UserRole>() { UserRole.Create(Role.Admin.Id) },
                email: EmailUtils.CreateEmail("system_admin@email.com"),
                passwordHash: _hasher.Hash("admin123")
            ),
            "admin123"
        ),

        [UserSeedType.OTHER_ADMIN] = (
            UserUtils.CreateUser
            (
                id: UserId.Create(-2),
                name: "other admin",
                roles: new HashSet<UserRole>() { UserRole.Create(Role.Admin.Id) },
                email: EmailUtils.CreateEmail("other_admin@email.com"),
                passwordHash: _hasher.Hash("other-admin123")
            ),
            "other-admin123"
        ),

        [UserSeedType.CUSTOMER] = (
            UserUtils.CreateUser(
                id: UserId.Create(-3),
                name: "normal user",
                email: EmailUtils.CreateEmail("user_normal@email.com"),
                roles: new HashSet<UserRole>() { UserRole.Create(Role.Customer.Id) },
                passwordHash: _hasher.Hash("customer123")
            ),
            "customer123"
        ),

        [UserSeedType.CUSTOMER_WITH_ADDRESS] = (
            UserUtils.CreateUser(
                id: UserId.Create(-4),
                name: "user with address",
                email: EmailUtils.CreateEmail("user_address@email.com"),
                roles: new HashSet<UserRole>() { UserRole.Create(Role.Customer.Id) },
                addresses: new HashSet<Address>() { AddressUtils.CreateAddress() },
                passwordHash: _hasher.Hash("customer-with-address-123")
            ),
            "customer-with-address-123"
        ),

        [UserSeedType.CUSTOMER_INACTIVE] = (
            UserUtils.CreateUser(
                id: UserId.Create(-5),
                name: "inactive user",
                email: EmailUtils.CreateEmail("user_inactive@email.com"),
                roles: new HashSet<UserRole>() { UserRole.Create(Role.Customer.Id) },
                passwordHash: _hasher.Hash("customer-inactive-123"),
                active: false
            ),
            "customer-inactive-123"
        ),
    };

    /// <inheritdoc/>
    public override int Order => 10;

    private static readonly Dictionary<UserSeedType, User> _users = _usersWithCredentials.ToDictionary(u => u.Key, u => u.Value.User);

    /// <summary>
    /// Initiates a new instance of the <see cref="UserSeed"/> class.
    /// </summary>
    public UserSeed() : base(_users)
    {
    }

    /// <summary>
    /// Retrieves the authentication credentials for a seed user.
    /// </summary>
    /// <param name="seedUser">The user to get the credentials from.</param>
    /// <returns>A tuple containing the email and password.</returns>
    public (string Email, string Password) GetUserAuthenticationCredentials(UserSeedType seedUser)
    {
        var user = _usersWithCredentials[seedUser];

        return (user.User.Email.ToString(), user.Password);
    }
    /// <summary>
    /// Retrieves all the seed user credentials. By default retrieves only active users.
    /// </summary>
    /// <param name="filter">Filter the users credentials based on a predicate.</param>
    /// <returns>A tuple of email and password for each seed user.</returns>
    public IReadOnlyList<(string Email, string Password)> ListUsersCredentials(Func<User, bool>? filter = null)
    {

        return filter != null ?
            _usersWithCredentials.Values.Where(u => filter(u.User)).Select(u => (u.User.Email.ToString(), u.Password)).ToList() :
            _usersWithCredentials.Values.Select(u => (u.User.Email.ToString(), u.Password)).ToList();
    }

    /// <inheritdoc/>
    public override async Task SeedAsync(ECommerceDbContext context)
    {
        await context.Users.AddRangeAsync(ListAll());
    }
}
