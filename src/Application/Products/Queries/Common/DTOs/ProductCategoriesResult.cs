using Domain.ProductAggregate.Enumerations;

namespace Application.Products.Queries.Common.DTOs;

/// <summary>
/// Defines a product category list data transfer object to communicate with higher layers.
/// </summary>
/// <param name="Categories">The product categories to be passed.</param>
public record ProductCategoriesResult(IEnumerable<Category> Categories);
