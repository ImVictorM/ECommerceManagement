using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate;
using Domain.CarrierAggregate.ValueObjects;
using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.ShipmentAggregate.ValueObjects;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;
using SharedKernel.UnitTests.TestUtils.Extensions;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Shipment"/> class.
/// </summary>
public static class ShipmentUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="Shipment"/> class.
    /// </summary>
    /// <param name="id">The shipment id.</param>
    /// <param name="orderId">The order id.</param>
    /// <param name="carrierId">The carrier id.</param>
    /// <param name="shippingMethodId">The shipping method id.</param>
    /// <param name="deliveryAddress">The delivery address.</param>
    /// <returns>A new instance of the <see cref="Shipment"/> class.</returns>
    public static Shipment CreateShipment(
        ShipmentId? id = null,
        OrderId? orderId = null,
        CarrierId? carrierId = null,
        ShippingMethodId? shippingMethodId = null,
        Address? deliveryAddress = null
    )
    {
        var shipment = Shipment.Create(
            orderId ?? OrderId.Create(NumberUtils.CreateRandomLong()),
            carrierId ?? CarrierId.Create(NumberUtils.CreateRandomLong()),
            shippingMethodId ?? ShippingMethodId.Create(NumberUtils.CreateRandomLong()),
            deliveryAddress ?? AddressUtils.CreateAddress()
       );

        if (id != null)
        {
            shipment.SetIdUsingReflection(id);
        }

        return shipment;
    }
}
