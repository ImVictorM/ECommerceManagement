using Domain.ShipmentAggregate.ValueObjects;

using SharedKernel.ValueObjects;

namespace Application.Orders.DTOs;

/// <summary>
/// Represents an order shipment.
/// </summary>
/// <param name="ShipmentId">The shipment id.</param>
/// <param name="Status">The shipment status.</param>
/// <param name="DeliveryAddress">The delivery address.</param>
/// <param name="ShippingMethod">The shipping method.</param>
public record OrderShipmentResult(
    ShipmentId ShipmentId,
    string Status,
    Address DeliveryAddress,
    OrderShippingMethodResult ShippingMethod
);
