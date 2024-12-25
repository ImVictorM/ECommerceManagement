using Contracts.Common;

namespace Contracts.Products;

/// <summary>
/// Represents a request object containing a new product data.
/// </summary>
/// <param name="Name">The new product name.</param>
/// <param name="Description">The new product description.</param>
/// <param name="InitialQuantity">The new product initial quantity to be placed in the product's inventory.</param>
/// <param name="BasePrice">The new product base price.</param>
/// <param name="Categories">Categories the new product belongs to.</param>
/// <param name="Images">The new product images.</param>
/// <param name="InitialDiscounts">The new product initial discounts (optional).</param>
public record CreateProductRequest(
    string Name,
    string Description,
    int InitialQuantity,
    decimal BasePrice,
    IEnumerable<long> Categories,
    IEnumerable<Uri> Images,
    IEnumerable<DiscountContract>? InitialDiscounts = null
);
