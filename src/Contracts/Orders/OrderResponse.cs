namespace Contracts.Orders;

/// <summary>
/// Represents an order response.
/// </summary>
/// <param name="Id">The order identifier.</param>
/// <param name="OwnerId">The order owner identifier.</param>
/// <param name="Description">The order description.</param>
/// <param name="Status">The order status.</param>
/// <param name="Total">The order total.</param>
public record OrderResponse(
    string Id,
    string OwnerId,
    string Description,
    string Status,
    decimal Total
);
