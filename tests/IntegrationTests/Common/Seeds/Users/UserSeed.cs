using Application.Common.Security.Authentication;

using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

using Infrastructure.Common.Persistence;

using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.Users;

/// <summary>
/// Provides seed data for users in the database.
/// </summary>
public sealed class UserSeed : DataSeed<UserSeedType, User, UserId>, IUserSeed
{
    /// <inheritdoc/>
    public override int Order => 10;

    /// <summary>
    /// Initiates a new instance of the <see cref="UserSeed"/> class.
    /// </summary>
    public UserSeed(
        IUserCredentialsProvider credentialsProvider,
        IPasswordHasher passwordHasher
    )
        : base(CreateUserData(credentialsProvider, passwordHasher))
    {
    }

    private static Dictionary<UserSeedType, User> CreateUserData(
        IUserCredentialsProvider credentialsProvider,
        IPasswordHasher passwordHasher
    )
    {
        return new()
        {
            [UserSeedType.ADMIN] = (
               UserUtils.CreateAdmin
               (
                   id: UserId.Create(-1),
                   name: "admin",
                   email: EmailUtils.CreateEmail(
                       credentialsProvider.GetEmail(UserSeedType.ADMIN)
                   ),
                   passwordHash: passwordHasher.Hash(
                       credentialsProvider.GetPassword(UserSeedType.ADMIN)
                   )
               )
            ),

            [UserSeedType.OTHER_ADMIN] = (
                UserUtils.CreateAdmin
                (
                   id: UserId.Create(-2),
                   name: "other admin",
                   email: EmailUtils.CreateEmail(
                       credentialsProvider.GetEmail(UserSeedType.OTHER_ADMIN)
                   ),
                   passwordHash: passwordHasher.Hash(
                       credentialsProvider.GetPassword(UserSeedType.OTHER_ADMIN)
                   )
                )
            ),

            [UserSeedType.CUSTOMER] = (
               UserUtils.CreateCustomer(
                   id: UserId.Create(-3),
                   name: "normal user",
                   email: EmailUtils.CreateEmail(
                       credentialsProvider.GetEmail(UserSeedType.CUSTOMER)
                   ),
                   passwordHash: passwordHasher.Hash(
                       credentialsProvider.GetPassword(UserSeedType.CUSTOMER)
                   )
               )
           ),

            [UserSeedType.CUSTOMER_WITH_ADDRESS] = (
               UserUtils.CreateCustomer(
                   id: UserId.Create(-4),
                   name: "user with address",
                   email: EmailUtils.CreateEmail(
                       credentialsProvider.GetEmail(UserSeedType.CUSTOMER_WITH_ADDRESS)
                   ),
                   addresses: new HashSet<Address>() { AddressUtils.CreateAddress() },
                   passwordHash: passwordHasher.Hash(
                       credentialsProvider.GetPassword(UserSeedType.CUSTOMER_WITH_ADDRESS)
                   )
               )
           ),

            [UserSeedType.CUSTOMER_INACTIVE] = (
               UserUtils.CreateCustomer(
                   id: UserId.Create(-5),
                   name: "inactive user",
                   email: EmailUtils.CreateEmail(
                       credentialsProvider.GetEmail(UserSeedType.CUSTOMER_INACTIVE)
                   ),
                   passwordHash: passwordHasher.Hash(
                       credentialsProvider.GetPassword(UserSeedType.CUSTOMER_INACTIVE)
                   ),
                   active: false
               )
           ),
        };
    }

    /// <inheritdoc/>
    public override async Task SeedAsync(IECommerceDbContext context)
    {
        await context.Users.AddRangeAsync(ListAll());
    }
}
