using Application.Common.DTOs.Pagination;
using Application.Products.DTOs.Filters;
using Application.Products.Queries.GetProducts;

namespace Application.UnitTests.Products.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetProductsQuery"/> query.
/// </summary>
public static class GetProductsQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetProductsQuery"/> class.
    /// </summary>
    /// <param name="paginationParams">The pagination parameters.</param>
    /// <param name="filters">The product filters.</param>
    /// <returns>
    /// A new instance of the <see cref="GetProductsQuery"/> class.
    /// </returns>
    public static GetProductsQuery CreateQuery(
        PaginationParams? paginationParams = null,
        ProductFilters? filters = null
    )
    {
        return new GetProductsQuery(
            paginationParams ?? new(),
            filters ?? new()
        );
    }
}
