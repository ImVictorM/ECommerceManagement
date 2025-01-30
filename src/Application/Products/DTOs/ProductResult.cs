using Domain.ProductAggregate;

namespace Application.Products.DTOs;

/// <summary>
/// Defines a product data transfer object to communicate with higher layers.
/// </summary>
/// <param name="Product">The product to be passed.</param>
/// <param name="Categories">The product categories.</param>
/// <param name="PriceWithDiscount">The product price with discounts.</param>
public record ProductResult(
    Product Product,
    IEnumerable<string> Categories,
    decimal PriceWithDiscount
);
