using Domain.OrderAggregate;
using Domain.PaymentAggregate.ValueObjects;

namespace Application.Orders.DTOs;

/// <summary>
/// Represents an order detailed query result.
/// </summary>
/// <param name="Order">The order.</param>
/// <param name="OrderShipment">The order shipment details.</param>
/// <param name="PaymentId">The order payment id.</param>
public record OrderDetailedQueryResult(
    Order Order,
    OrderShipmentResult OrderShipment,
    PaymentId PaymentId
);
