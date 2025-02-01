using Domain.ShippingMethodAggregate;
using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

namespace IntegrationTests.TestUtils.Seeds;

/// <summary>
/// Maps the available shipping methods in the seed.
/// </summary>
public enum SeedAvailableShippingMethods
{
    /// <summary>
    /// Represents a free shipping method
    /// </summary>
    FREE,
    /// <summary>
    /// Represents an express shipping method.
    /// </summary>
    EXPRESS,
}

/// <summary>
/// Contains shipping method seed data.
/// </summary>
public static class ShippingMethodSeed
{
    private static readonly Dictionary<SeedAvailableShippingMethods, ShippingMethod> _shippingMethods = new()
    {
        [SeedAvailableShippingMethods.FREE] = ShippingMethodUtils.CreateShippingMethod
        (
            id: ShippingMethodId.Create(-1),
            name: "Free Shipping",
            price: 0m,
            estimatedDeliveryDays: 12
        ),
        [SeedAvailableShippingMethods.EXPRESS] = ShippingMethodUtils.CreateShippingMethod
        (
            id: ShippingMethodId.Create(-2),
            name: "Express Shipping",
            price: 20m,
            estimatedDeliveryDays: 5
        )
    };

    /// <summary>
    /// Retrieves a seed shipping method by type.
    /// </summary>
    public static ShippingMethod GetSeedShippingMethod(SeedAvailableShippingMethods methodType)
    {
        return _shippingMethods[methodType];
    }

    /// <summary>
    /// Lists all seed shipping methods.
    /// </summary>
    public static IReadOnlyList<ShippingMethod> ListShippingMethods()
    {
        return [.. _shippingMethods.Values];
    }
}
