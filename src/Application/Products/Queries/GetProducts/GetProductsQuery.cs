using Application.Products.DTOs;

using MediatR;

namespace Application.Products.Queries.GetProducts;

/// <summary>
/// Query to get all active products.
/// </summary>
/// <param name="Page">The current products page.</param>
/// <param name="PageSize">The quantity of products to be fetched.</param>
/// <param name="Categories">The categories the product should have.</param>
public record GetProductsQuery(
    int? Page,
    int? PageSize,
    IEnumerable<string>? Categories = null
) : IRequest<IEnumerable<ProductResult>>;
