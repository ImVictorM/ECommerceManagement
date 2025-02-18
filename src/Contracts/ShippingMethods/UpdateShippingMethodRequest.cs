namespace Contracts.ShippingMethods;

/// <summary>
/// Represents a request to update a shipping method.
/// </summary>
/// <param name="Name">The new shipping method name.</param>
/// <param name="Price">The new shipping method price.</param>
/// <param name="EstimatedDeliveryDays">The new shipping method estimated delivery days.</param>
public record UpdateShippingMethodRequest(
    string Name,
    decimal Price,
    int EstimatedDeliveryDays
);
