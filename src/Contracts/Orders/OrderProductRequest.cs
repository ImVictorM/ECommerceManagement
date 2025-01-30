namespace Contracts.Orders;

/// <summary>
/// Represents a request order product.
/// </summary>
/// <param name="ProductId">The product id.</param>
/// <param name="Quantity">The product quantity.</param>
public record OrderProductRequest(
    string ProductId,
    int Quantity
);
