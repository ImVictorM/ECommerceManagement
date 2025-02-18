using Domain.ShippingMethodAggregate;
using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using Infrastructure.Common.Persistence;

using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.ShippingMethods;

/// <summary>
/// Provides seed data for shipping methods in the database.
/// </summary>
public sealed class ShippingMethodSeed : DataSeed<ShippingMethodSeedType, ShippingMethod>
{
    /// <inheritdoc/>
    public override int Order => 10;

    private static Dictionary<ShippingMethodSeedType, ShippingMethod> _shippingMethods => new()
    {
        [ShippingMethodSeedType.FREE] = ShippingMethodUtils.CreateShippingMethod
        (
            id: ShippingMethodId.Create(-1),
            name: "Free Shipping",
            price: 0m,
            estimatedDeliveryDays: 12
        ),
        [ShippingMethodSeedType.EXPRESS] = ShippingMethodUtils.CreateShippingMethod
        (
            id: ShippingMethodId.Create(-2),
            name: "Express Shipping",
            price: 20m,
            estimatedDeliveryDays: 5
        )
    };

    /// <summary>
    /// Initiates a new instance of the <see cref="ShippingMethodSeed"/> class.
    /// </summary>
    public ShippingMethodSeed() : base(_shippingMethods)
    {
    }

    /// <inheritdoc/>
    public override async Task SeedAsync(ECommerceDbContext context)
    {
        await context.ShippingMethods.AddRangeAsync(ListAll());
    }
}
