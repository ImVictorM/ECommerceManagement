using Application.Common.DTOs.Pagination;
using Application.Products.DTOs.Filters;
using Application.Products.DTOs.Results;

using MediatR;

namespace Application.Products.Queries.GetProducts;

/// <summary>
/// Represents a query to retrieve all the active products.
/// The query has support for pagination and filters.
/// </summary>
/// <param name="Filters">The filtering criteria.</param>
/// <param name="PaginationParams">The pagination parameters.</param>
public record GetProductsQuery(
    PaginationParams PaginationParams,
    ProductFilters Filters
) : IRequest<IReadOnlyList<ProductResult>>;
