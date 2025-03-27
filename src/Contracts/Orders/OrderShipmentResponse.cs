using Contracts.Common;

namespace Contracts.Orders;

/// <summary>
/// Represents an order shipment response.
/// </summary>
/// <param name="ShipmentId">The shipment identifier.</param>
/// <param name="Status">The shipment status.</param>
/// <param name="DeliveryAddress">The shipment delivery address.</param>
/// <param name="ShippingMethod">The shipping method.</param>
public record OrderShipmentResponse(
    string ShipmentId,
    string Status,
    AddressContract DeliveryAddress,
    OrderShippingMethodResponse ShippingMethod
);
