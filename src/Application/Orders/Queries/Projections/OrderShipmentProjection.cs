using Domain.ShipmentAggregate.Enumerations;
using Domain.ShipmentAggregate.ValueObjects;

using SharedKernel.ValueObjects;

namespace Application.Orders.Queries.Projections;

/// <summary>
/// Represents an order shipment projection.
/// </summary>
/// <param name="Id">The shipment identifier.</param>
/// <param name="ShipmentStatus">The shipment status.</param>
/// <param name="DeliveryAddress">The shipment delivery address.</param>
/// <param name="ShippingMethod">The shipping method.</param>
public record OrderShipmentProjection(
    ShipmentId Id,
    ShipmentStatus ShipmentStatus,
    Address DeliveryAddress,
    OrderShippingMethodProjection ShippingMethod
);
