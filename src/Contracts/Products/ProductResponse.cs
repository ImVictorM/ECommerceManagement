namespace Contracts.Products;

/// <summary>
/// Represents a product response.
/// </summary>
/// <param name="Id">The product id.</param>
/// <param name="Name">The product name.</param>
/// <param name="Description">The product description.</param>
/// <param name="BasePrice">The product original price.</param>
/// <param name="PriceWithDiscount">The product price with discounts applied.</param>
/// <param name="QuantityAvailable">The product quantity in inventory.</param>
/// <param name="Categories">The product categories.</param>
/// <param name="Images">The product images.</param>
public record ProductResponse(
    string Id,
    string Name,
    string Description,
    decimal BasePrice,
    decimal PriceWithDiscount,
    int QuantityAvailable,
    IEnumerable<string> Categories,
    IEnumerable<Uri> Images
);
