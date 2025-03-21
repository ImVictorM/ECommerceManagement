namespace Contracts.Products;

/// <summary>
/// Represents a request to update a product.
/// </summary>
/// <param name="Name">The new product name.</param>
/// <param name="Description">The new product description.</param>
/// <param name="BasePrice">The new product base price.</param>
/// <param name="Images">The new product images.</param>
/// <param name="CategoryIds">The new product categories.</param>
public record UpdateProductRequest(
    string Name,
    string Description,
    decimal BasePrice,
    IEnumerable<Uri> Images,
    IEnumerable<string> CategoryIds
);
