using Contracts.Products.Common;

namespace Contracts.Products;

/// <summary>
/// Defines a product response.
/// </summary>
/// <param name="Id">The product id.</param>
/// <param name="Name">The product name.</param>
/// <param name="Description">The product description.</param>
/// <param name="OriginalPrice">The product original price.</param>
/// <param name="PriceWithDiscount">The product price with discounts applied.</param>
/// <param name="QuantityAvailable">The product quantity in inventory.</param>
/// <param name="DiscountsApplied">The discounts applied that generated the price with discount.</param>
/// <param name="Categories">The product categories.</param>
/// <param name="Images">The product images.</param>
public record ProductResponse(
    string Id,
    string Name,
    string Description,
    decimal OriginalPrice,
    decimal PriceWithDiscount,
    int QuantityAvailable,
    IEnumerable<Discount> DiscountsApplied,
    IEnumerable<string> Categories,
    IEnumerable<Uri> Images
);
