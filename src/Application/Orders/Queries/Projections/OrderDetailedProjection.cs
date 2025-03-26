using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Application.Orders.Queries.Projections;

/// <summary>
/// Represents an order detailed projection.
/// </summary>
/// <param name="Id">The order identifier.</param>
/// <param name="OwnerId">The order owner identifier.</param>
/// <param name="Description">The order description.</param>
/// <param name="OrderStatus">The order status.</param>
/// <param name="Total">The order total.</param>
/// <param name="Shipment">The order shipment.</param>
/// <param name="PaymentId">The order payment identifier.</param>
public record OrderDetailedProjection(
    OrderId Id,
    UserId OwnerId,
    string Description,
    OrderStatus OrderStatus,
    decimal Total,
    OrderShipmentProjection Shipment,
    PaymentId PaymentId
);
