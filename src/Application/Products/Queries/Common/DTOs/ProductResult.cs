using Domain.ProductAggregate;

namespace Application.Products.Queries.Common.DTOs;

/// <summary>
/// Defines a product data transfer object to communicate with higher layers.
/// </summary>
/// <param name="Product">The product to be passed.</param>
public record ProductResult(Product Product);
