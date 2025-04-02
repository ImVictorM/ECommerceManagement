namespace Contracts.Products;

/// <summary>
/// Represents a request to increase the stock quantity of a product.
/// </summary>
/// <param name="QuantityToAdd">
/// The quantity to add.
/// </param>
public record AddStockRequest(int QuantityToAdd);
