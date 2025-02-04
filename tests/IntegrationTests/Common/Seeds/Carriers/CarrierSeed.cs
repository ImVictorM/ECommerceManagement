using Domain.UnitTests.TestUtils;
using Domain.CarrierAggregate;
using Domain.CarrierAggregate.ValueObjects;

using SharedKernel.ValueObjects;

using Infrastructure.Security.Authentication;
using Infrastructure.Common.Persistence;

using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.Carriers;

/// <summary>
/// Provides seed data for carriers in the database.
/// </summary>
public sealed class CarrierSeed : DataSeed<CarrierSeedType, Carrier>
{
    private static readonly PasswordHasher _hasher = new();

    private static Dictionary<CarrierSeedType, Carrier> _carriers => new()
    {
        [CarrierSeedType.INTERNAL] = CarrierUtils.CreateCarrier
        (
            id: CarrierId.Create(-1),
            name: "ECommerceManagementCarrier",
            email: Email.Create("carrier@email.com"),
            passwordHash: _hasher.Hash("carrier123"),
            phone: "19859284294"
        )
    };

    /// <inheritdoc/>
    public override int Order => 10;

    /// <summary>
    /// Initiates a new instance of the <see cref="CarrierSeed"/> class.
    /// </summary>
    public CarrierSeed() : base(_carriers)
    {
    }

    /// <inheritdoc/>
    public override async Task SeedAsync(ECommerceDbContext context)
    {
        await context.Carriers.AddRangeAsync(ListAll());
    }
}
