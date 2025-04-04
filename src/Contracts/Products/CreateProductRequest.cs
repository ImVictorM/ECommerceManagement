namespace Contracts.Products;

/// <summary>
/// Represents a request to create a new product.
/// </summary>
/// <param name="Name">The new product name.</param>
/// <param name="Description">The new product description.</param>
/// <param name="InitialQuantity">
/// The new product initial quantity to be placed in the product's inventory.
/// </param>
/// <param name="BasePrice">The new product base price.</param>
/// <param name="CategoryIds">The categories the new product belongs to.</param>
/// <param name="Images">The new product images.</param>
public record CreateProductRequest(
    string Name,
    string Description,
    int InitialQuantity,
    decimal BasePrice,
    IEnumerable<string> CategoryIds,
    IEnumerable<Uri> Images
);
