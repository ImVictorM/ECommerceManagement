using Domain.UnitTests.TestUtils;
using Domain.CarrierAggregate;
using Domain.CarrierAggregate.ValueObjects;

namespace IntegrationTests.TestUtils.Seeds;

/// <summary>
/// Maps the available carriers in the seed.
/// </summary>
public enum SeedAvailableCarriers
{
    /// <summary>
    /// Represents an internal carrier.
    /// </summary>
    INTERNAL,
}

/// <summary>
/// Contains shipping method seed data.
/// </summary>
public static class CarrierSeed
{
    private static readonly Dictionary<SeedAvailableCarriers, Carrier> _carriers = new()
    {
        [SeedAvailableCarriers.INTERNAL] = CarrierUtils.CreateCarrier
        (
            id: CarrierId.Create(-1),
            name: "ECommerceManagementCarrier"
        )
    };

    /// <summary>
    /// Retrieves a seed carrier by type.
    /// </summary>
    /// <param name="carrierType">The carrier type.</param>
    public static Carrier GetSeedCarrier(SeedAvailableCarriers carrierType)
    {
        return _carriers[carrierType];
    }

    /// <summary>
    /// Lists all seed carriers.
    /// </summary>
    public static IReadOnlyList<Carrier> ListCarriers()
    {
        return [.. _carriers.Values];
    }
}
