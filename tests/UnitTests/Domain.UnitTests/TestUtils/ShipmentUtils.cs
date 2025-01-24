using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate;

using Bogus;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Shipment"/> class.
/// </summary>
public static class ShipmentUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="Shipment"/> class.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <param name="accountable">The accountable.</param>
    /// <returns>A new instance of the <see cref="Shipment"/> class.</returns>
    public static Shipment CreateShipment(
        OrderId? orderId = null,
        string? accountable = null
    )
    {
        return Shipment.Create(
            orderId ?? OrderId.Create(_faker.Random.Long()),
            accountable ?? _faker.Name.FullName()
       );
    }
}
