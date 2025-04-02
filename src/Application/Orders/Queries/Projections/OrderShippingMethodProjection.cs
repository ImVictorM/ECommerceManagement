namespace Application.Orders.Queries.Projections;

/// <summary>
/// Represents an order shipping method projection.
/// </summary>
/// <param name="Name">The shipping method name.</param>
/// <param name="Price">The shipping method price.</param>
/// <param name="EstimatedDeliveryDays">
/// The shipping method estimated delivery days.
/// </param>
public record OrderShippingMethodProjection(
    string Name,
    decimal Price,
    int EstimatedDeliveryDays
);
