using Domain.OrderAggregate;

namespace Application.Orders.DTOs;

/// <summary>
/// Represents an order result.
/// </summary>
/// <param name="Order">The order.</param>
public record class OrderResult(Order Order);
