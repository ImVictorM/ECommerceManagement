using Domain.ProductAggregate;

namespace Application.Products.Queries.Common.DTOs;

/// <summary>
/// Defines a product list data transfer object to communicate with higher layers.
/// </summary>
/// <param name="Products">The products to be passed.</param>
public record ProductListResult(IEnumerable<Product> Products);
