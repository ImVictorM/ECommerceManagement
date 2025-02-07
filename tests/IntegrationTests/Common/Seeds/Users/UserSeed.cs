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
public sealed class UserSeed : DataSeed<UserSeedType, User>
{
    /// <inheritdoc/>
    public override int Order => 10;

    /// <summary>
    /// Initiates a new instance of the <see cref="UserSeed"/> class.
    /// </summary>
    public UserSeed(ICredentialsProvider<UserSeedType> credentialsProvider, IPasswordHasher passwordHasher)
        : base(CreateUserData(credentialsProvider, passwordHasher))
    {
    }

    private static Dictionary<UserSeedType, User> CreateUserData(
        ICredentialsProvider<UserSeedType> credentialsProvider,
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
                   email: EmailUtils.CreateEmail(credentialsProvider.GetCredentials(UserSeedType.ADMIN).Email),
                   passwordHash: passwordHasher.Hash(credentialsProvider.GetCredentials(UserSeedType.ADMIN).Password)
               )
            ),

            [UserSeedType.OTHER_ADMIN] = (
                UserUtils.CreateAdmin
                (
                   id: UserId.Create(-2),
                   name: "other admin",
                   email: EmailUtils.CreateEmail(credentialsProvider.GetCredentials(UserSeedType.OTHER_ADMIN).Email),
                   passwordHash: passwordHasher.Hash(credentialsProvider.GetCredentials(UserSeedType.OTHER_ADMIN).Password)
                )
            ),

            [UserSeedType.CUSTOMER] = (
               UserUtils.CreateCustomer(
                   id: UserId.Create(-3),
                   name: "normal user",
                   email: EmailUtils.CreateEmail(credentialsProvider.GetCredentials(UserSeedType.CUSTOMER).Email),
                   passwordHash: passwordHasher.Hash(credentialsProvider.GetCredentials(UserSeedType.CUSTOMER).Password)
               )
           ),

            [UserSeedType.CUSTOMER_WITH_ADDRESS] = (
               UserUtils.CreateCustomer(
                   id: UserId.Create(-4),
                   name: "user with address",
                   email: EmailUtils.CreateEmail(credentialsProvider.GetCredentials(UserSeedType.CUSTOMER_WITH_ADDRESS).Email),
                   addresses: new HashSet<Address>() { AddressUtils.CreateAddress() },
                   passwordHash: passwordHasher.Hash(credentialsProvider.GetCredentials(UserSeedType.CUSTOMER_WITH_ADDRESS).Password)
               )
           ),

            [UserSeedType.CUSTOMER_INACTIVE] = (
               UserUtils.CreateCustomer(
                   id: UserId.Create(-5),
                   name: "inactive user",
                   email: EmailUtils.CreateEmail(credentialsProvider.GetCredentials(UserSeedType.CUSTOMER_INACTIVE).Email),
                   passwordHash: passwordHasher.Hash(credentialsProvider.GetCredentials(UserSeedType.CUSTOMER_INACTIVE).Password),
                   active: false
               )
           ),
        };
    }

    /// <inheritdoc/>
    public override async Task SeedAsync(ECommerceDbContext context)
    {
        await context.Users.AddRangeAsync(ListAll());
    }
}
