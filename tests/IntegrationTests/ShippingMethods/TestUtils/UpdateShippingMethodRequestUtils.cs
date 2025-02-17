using Contracts.ShippingMethods;

using Bogus;

namespace IntegrationTests.ShippingMethods.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdateShippingMethodRequest"/> class.
/// </summary>
public static class UpdateShippingMethodRequestUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="UpdateShippingMethodRequestUtils"/> class.
    /// </summary>
    /// <param name="name">The shipping method name.</param>
    /// <param name="price">The shipping method price.</param>
    /// <param name="estimatedDeliveryDays">The shipping method estimated delivery days.</param>
    /// <returns>A new instance of the <see cref="UpdateShippingMethodRequestUtils"/> class.</returns>
    public static UpdateShippingMethodRequest CreateRequest(
        string? name = null,
        decimal? price = null,
        int? estimatedDeliveryDays = null
    )
    {
        return new UpdateShippingMethodRequest(
            name ?? "IntegrationTestShippingMethod",
            price ?? _faker.Random.Decimal(0, 50),
            estimatedDeliveryDays ?? _faker.Random.Int(1, 7)
        );
    }
}
