namespace Contracts.Orders;

/// <summary>
/// Represents an order product response.
/// </summary>
/// <param name="ProductId">The product id.</param>
/// <param name="Quantity">The product quantity.</param>
/// <param name="BasePrice">The product base price.</param>
/// <param name="PurchasedPrice">The product purchase price.</param>
public record OrderLineItemResponse(
    string ProductId,
    int Quantity,
    decimal BasePrice,
    decimal PurchasedPrice
);
