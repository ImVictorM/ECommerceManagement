namespace Contracts.Orders;

/// <summary>
/// Represents an order shipping method response.
/// </summary>
/// <param name="Name">The shipping method name.</param>
/// <param name="Price">The shipping price.</param>
/// <param name="EstimatedDeliveryDays">The estimated delivery days.</param>
public record OrderShippingMethodResponse(
    string Name,
    decimal Price,
    int EstimatedDeliveryDays
);
