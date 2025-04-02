namespace Contracts.ShippingMethods;

/// <summary>
/// Represents a request to create a new shipping method.
/// </summary>
/// <param name="Name">The shipping method name.</param>
/// <param name="Price">The shipping method price.</param>
/// <param name="EstimatedDeliveryDays"
/// >The shipping method estimated delivery days.
/// </param>
public record CreateShippingMethodRequest(
    string Name,
    decimal Price,
    int EstimatedDeliveryDays
);
