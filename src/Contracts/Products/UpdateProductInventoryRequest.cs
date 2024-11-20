namespace Contracts.Products;

/// <summary>
/// Represents a request to update a product's inventory.
/// </summary>
/// <param name="QuantityToIncrement">The quantity to increment in the inventory.</param>
public record UpdateProductInventoryRequest(int QuantityToIncrement);
