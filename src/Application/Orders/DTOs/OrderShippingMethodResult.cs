namespace Application.Orders.DTOs;

/// <summary>
/// Represents an order shipping method.
/// </summary>
/// <param name="Name">The shipping method name.</param>
/// <param name="Price">The shipping price.</param>
/// <param name="EstimatedDeliveryDays">The estimated delivery days.</param>
public record OrderShippingMethodResult(string Name, decimal Price, int EstimatedDeliveryDays);
