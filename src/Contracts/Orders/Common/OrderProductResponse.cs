namespace Contracts.Orders.Common;

/// <summary>
/// Represents an order product response.
/// </summary>
/// <param name="ProductId">The product id.</param>
/// <param name="Quantity">The product quantity.</param>
/// <param name="BasePrice">The product base price.</param>
/// <param name="PurchasedPrice">The product purchase price.</param>
public record OrderProductResponse(
    string ProductId,
    string Quantity,
    decimal BasePrice,
    decimal PurchasedPrice
);
