using Application.ShippingMethods.Commands.CreateShippingMethod;

using Bogus;

namespace Application.UnitTests.ShippingMethods.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="CreateShippingMethodCommand"/> class.
/// </summary>
public static class CreateShippingMethodCommandUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="CreateShippingMethodCommand"/>
    /// class.
    /// </summary>
    /// <param name="name">The shipping method name.</param>
    /// <param name="price">The shipping method price.</param>
    /// <param name="estimatedDeliveryDays">
    /// The shipping method estimated delivery days.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="CreateShippingMethodCommand"/> class.
    /// </returns>
    public static CreateShippingMethodCommand CreateCommand(
        string? name = null,
        decimal? price = null,
        int? estimatedDeliveryDays = null
    )
    {
        return new CreateShippingMethodCommand(
            name ?? "ECommerceDeliveryMethod",
            price ?? _faker.Random.Decimal(0, 50),
            estimatedDeliveryDays ?? _faker.Random.Int(1, 7)
        );
    }
}
