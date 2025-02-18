using Contracts.Common;

namespace Contracts.Orders;

/// <summary>
/// Represents and order shipment response.
/// </summary>
/// <param name="ShipmentId">The shipment id.</param>
/// <param name="Status">The shipment status.</param>
/// <param name="DeliveryAddress">The shipment delivery address.</param>
/// <param name="ShippingMethod">The shipping method.</param>
public record OrderShipmentResponse(
    string ShipmentId,
    string Status,
    AddressContract DeliveryAddress,
    OrderShippingMethodResponse ShippingMethod
);
