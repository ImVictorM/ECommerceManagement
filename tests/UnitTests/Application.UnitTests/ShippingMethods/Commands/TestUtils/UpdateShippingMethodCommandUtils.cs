using Application.ShippingMethods.Commands.UpdateShippingMethod;

using Domain.UnitTests.TestUtils;

using Bogus;

namespace Application.UnitTests.ShippingMethods.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdateShippingMethodCommand"/> class.
/// </summary>
public static class UpdateShippingMethodCommandUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="UpdateShippingMethodCommand"/>
    /// class.
    /// </summary>
    /// <param name="shippingMethodId">The shipping method identifier.</param>
    /// <param name="name">The shipping method name.</param>
    /// <param name="price">The shipping method price.</param>
    /// <param name="estimatedDeliveryDays">
    /// The shipping method estimated delivery days.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="UpdateShippingMethodCommand"/> class.
    /// </returns>
    public static UpdateShippingMethodCommand CreateCommand(
        string? shippingMethodId = null,
        string? name = null,
        decimal? price = null,
        int? estimatedDeliveryDays = null
    )
    {
        return new UpdateShippingMethodCommand(
            shippingMethodId ?? NumberUtils.CreateRandomLongAsString(),
            name ?? "UpdatedDeliveryMethod",
            price ?? _faker.Random.Decimal(0, 50),
            estimatedDeliveryDays ?? _faker.Random.Int(1, 7)
        );
    }
}
