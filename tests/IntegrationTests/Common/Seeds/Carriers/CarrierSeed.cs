using Application.Common.Security.Authentication;

using Domain.UnitTests.TestUtils;
using Domain.CarrierAggregate;
using Domain.CarrierAggregate.ValueObjects;

using SharedKernel.ValueObjects;

using Infrastructure.Common.Persistence;

using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.Carriers;

/// <summary>
/// Provides seed data for carriers in the database.
/// </summary>
public sealed class CarrierSeed
    : DataSeed<CarrierSeedType, Carrier, CarrierId>, ICarrierSeed
{
    /// <inheritdoc/>
    public override int Order => 10;

    /// <summary>
    /// Initiates a new instance of the <see cref="CarrierSeed"/> class.
    /// </summary>
    public CarrierSeed(
        IPasswordHasher passwordHasher,
        ICarrierCredentialsProvider credentialsProvider
    )
        : base(CreateSeedData(passwordHasher, credentialsProvider))
    {
    }

    private static Dictionary<CarrierSeedType, Carrier> CreateSeedData(
        IPasswordHasher passwordHasher,
        ICredentialsProvider<CarrierSeedType> credentialsProvider
    )
    {
        return new()
        {
            [CarrierSeedType.INTERNAL] = CarrierUtils.CreateCarrier
            (
                id: CarrierId.Create(-1),
                name: "ECommerceManagementCarrier",
                email: Email.Create(
                    credentialsProvider.GetEmail(CarrierSeedType.INTERNAL)
                ),
                passwordHash: passwordHasher.Hash(
                    credentialsProvider.GetPassword(CarrierSeedType.INTERNAL)
                ),
                phone: "19859284294"
            )
        };
    }

    /// <inheritdoc/>
    public override async Task SeedAsync(IECommerceDbContext context)
    {
        await context.Carriers.AddRangeAsync(ListAll());
    }
}
