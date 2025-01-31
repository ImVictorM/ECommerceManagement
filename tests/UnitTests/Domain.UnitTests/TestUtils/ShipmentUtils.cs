using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate;
using Domain.CarrierAggregate.ValueObjects;
using Domain.ShippingMethodAggregate.ValueObjects;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Shipment"/> class.
/// </summary>
public static class ShipmentUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="Shipment"/> class.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <param name="carrierId">The carrier id.</param>
    /// <param name="shippingMethodId">The shipping method id.</param>
    /// <returns>A new instance of the <see cref="Shipment"/> class.</returns>
    public static Shipment CreateShipment(
        OrderId? orderId = null,
        CarrierId? carrierId = null,
        ShippingMethodId? shippingMethodId = null
    )
    {
        return Shipment.Create(
            orderId ?? OrderId.Create(NumberUtils.CreateRandomLong()),
            carrierId ?? CarrierId.Create(NumberUtils.CreateRandomLong()),
            shippingMethodId ?? ShippingMethodId.Create(NumberUtils.CreateRandomLong())
       );
    }
}
