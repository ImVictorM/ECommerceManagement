namespace Contracts.ShippingMethods;

/// <summary>
/// Represents a shipping method response.
/// </summary>
/// <param name="Id">The shipping method identifiers.</param>
/// <param name="Name">The shipping method name.</param>
/// <param name="Price">The shipping method price.</param>
/// <param name="EstimatedDeliveryDays">
/// The shipping method estimated delivery days.
/// </param>
public record ShippingMethodResponse(
    string Id,
    string Name,
    decimal Price,
    int EstimatedDeliveryDays
);
