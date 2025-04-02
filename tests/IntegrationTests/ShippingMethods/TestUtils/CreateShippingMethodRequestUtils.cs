using Contracts.ShippingMethods;

using Bogus;

namespace IntegrationTests.ShippingMethods.TestUtils;

/// <summary>
/// Utilities for the <see cref="CreateShippingMethodRequest"/> class.
/// </summary>
public static class CreateShippingMethodRequestUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="CreateShippingMethodRequest"/>
    /// class.
    /// </summary>
    /// <param name="name">The shipping method name.</param>
    /// <param name="price">The shipping method price.</param>
    /// <param name="estimatedDeliveryDays">
    /// The shipping method estimated delivery days.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="CreateShippingMethodRequest"/> class.
    /// </returns>
    public static CreateShippingMethodRequest CreateRequest(
        string? name = null,
        decimal? price = null,
        int? estimatedDeliveryDays = null
    )
    {
        return new CreateShippingMethodRequest(
            name ?? "IntegrationTestShippingMethod",
            price ?? _faker.Random.Decimal(0, 50),
            estimatedDeliveryDays ?? _faker.Random.Int(1, 7)
        );
    }
}
