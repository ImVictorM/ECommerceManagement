using Domain.ShippingMethodAggregate;
using Domain.ShippingMethodAggregate.ValueObjects;

using SharedKernel.UnitTests.TestUtils.Extensions;

using Bogus;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="ShippingMethod"/> class.
/// </summary>
public static class ShippingMethodUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="ShippingMethod"/> class.
    /// </summary>
    /// <param name="id">The shipping method identifier.</param>
    /// <param name="name">The shipping method name.</param>
    /// <param name="price">The shipping method price.</param>
    /// <param name="estimatedDeliveryDays">
    /// The shipping method estimated delivery days.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="ShippingMethod"/> class.
    /// </returns>
    public static ShippingMethod CreateShippingMethod(
        ShippingMethodId? id = null,
        string? name = null,
        decimal? price = null,
        int? estimatedDeliveryDays = null
    )
    {
        var shippingMethod = ShippingMethod.Create(
            name ?? "ECommerceDeliveryAgency",
            price ?? _faker.Random.Decimal(0, 50),
            estimatedDeliveryDays ?? _faker.Random.Int(1, 7)
        );

        if (id != null)
        {
            shippingMethod.SetIdUsingReflection(id);
        }

        return shippingMethod;
    }
}
