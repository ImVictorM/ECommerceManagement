using Domain.ProductAggregate;

namespace Application.Products.DTOs;

/// <summary>
/// Represents a product with the category names.
/// </summary>
/// <param name="Product">The product.</param>
/// <param name="CategoryNames">The product category names.</param>
public record ProductWithCategoriesQueryResult(
    Product Product,
    IEnumerable<string> CategoryNames
);
