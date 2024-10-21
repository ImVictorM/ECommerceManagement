using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.Entities;

namespace IntegrationTests.TestUtils.Seeds;

/// <summary>
/// 
/// </summary>
public static class UserSeed
{
    public static readonly string AdminPassword = "admin123";

    public static readonly string UserPassword = "user123";

    public static readonly User Admin = UserUtils.CreateUser(
        name: "admin",
        roleId: Role.Admin.Id,
        email: "system_admin@email.com",
        passwordHash: "6333824CC074E187E261A0CBBD91F9741B4D38A26E1519A93B4244BEAFC933B9",
        passwordSalt: "4FDE231393F2C8AECC2B26F356E3D89E"
    );

    public static readonly User User1 = UserUtils.CreateUser(
        name: "user 1",
        email: "user1@email.com",
        roleId: Role.Customer.Id
    );

    public static readonly User User2 = UserUtils.CreateUser(
        name: "user 2",
        email: "user2@email.com",
        roleId: Role.Customer.Id
    );

    public static IEnumerable<User> List()
    {
        return [Admin, User1, User2];
    }
}

