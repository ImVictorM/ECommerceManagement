namespace Contracts.Orders.Common;

/// <summary>
/// Represents an order product response.
/// </summary>
/// <param name="ProductId">The product id.</param>
/// <param name="Quantity">The product quantity.</param>
public record OrderProduct(string ProductId, string Quantity);
