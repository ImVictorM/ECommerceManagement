using Domain.OrderAggregate;

namespace Application.Orders.DTOs;

/// <summary>
/// Represents an order result.
/// </summary>
/// <param name="Order">The order.</param>
/// <param name="Payment">The order payment.</param>
/// <param name="Shipment">The order shipment.</param>
public record OrderDetailedResult(
    Order Order,
    OrderShipmentResult Shipment,
    OrderPaymentResult Payment
);
