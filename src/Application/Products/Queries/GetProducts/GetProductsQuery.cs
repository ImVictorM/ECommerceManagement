using Application.Products.Queries.Common.DTOs;
using MediatR;

namespace Application.Products.Queries.GetProducts;

/// <summary>
/// Query to get all active products.
/// </summary>
/// <param name="Limit">The quantity of products to be fetched.</param>
/// <param name="Categories">The categories the product should have.</param>
public record GetProductsQuery(
    int? Limit,
    IEnumerable<string>? Categories = null
) : IRequest<IEnumerable<ProductResult>>;
