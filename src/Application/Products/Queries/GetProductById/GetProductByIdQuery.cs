using Application.Products.DTOs.Results;

using MediatR;

namespace Application.Products.Queries.GetProductById;

/// <summary>
/// Query to get a product by identifier.
/// </summary>
/// <param name="Id">The product identifier.</param>
public record GetProductByIdQuery(string Id) : IRequest<ProductResult>;
