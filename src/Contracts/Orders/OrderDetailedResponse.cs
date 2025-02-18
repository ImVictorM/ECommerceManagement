namespace Contracts.Orders;

/// <summary>
/// Represents a detailed order response.
/// </summary>
/// <param name="Id">The order id.</param>
/// <param name="OwnerId">The order owner id.</param>
/// <param name="Description">The order description.</param>
/// <param name="Status">The order status.</param>
/// <param name="Total">The order total.</param>
/// <param name="Products">The order products.</param>
/// <param name="Payment">The order payment.</param>
/// <param name="Shipment">The order shipment.</param>
public record OrderDetailedResponse(
    string Id,
    string OwnerId,
    string Description,
    string Status,
    decimal Total,
    IEnumerable<OrderProductResponse> Products,
    OrderPaymentResponse Payment,
    OrderShipmentResponse Shipment
);
