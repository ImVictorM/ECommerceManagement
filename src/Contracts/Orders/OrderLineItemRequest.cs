namespace Contracts.Orders;

/// <summary>
/// Represents an order line item request.
/// </summary>
/// <param name="ProductId">The product identifier.</param>
/// <param name="Quantity">The product quantity.</param>
public record OrderLineItemRequest(
    string ProductId,
    int Quantity
);
