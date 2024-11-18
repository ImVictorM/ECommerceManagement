using Application.Products.Queries.Common.DTOs;
using MediatR;

namespace Application.Products.Queries.GetProducts;

/// <summary>
/// Query to get all products.
/// </summary>
/// <param name="Limit">The quantity of products to be fetched.</param>
public record GetProductsQuery(int Limit = 20) : IRequest<ProductListResult>;
