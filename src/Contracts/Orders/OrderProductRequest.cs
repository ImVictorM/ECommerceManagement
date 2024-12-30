namespace Contracts.Orders;

/// <summary>
/// Represents an order product request.
/// </summary>
/// <param name="ProductId">The product id.</param>
/// <param name="Quantity">The product quantity.</param>
public record OrderProductRequest(
    string ProductId,
    int Quantity
);
