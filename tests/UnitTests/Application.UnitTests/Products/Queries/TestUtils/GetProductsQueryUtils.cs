using Application.Products.Queries.GetProducts;

namespace Application.UnitTests.Products.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetProductsQuery"/> query.
/// </summary>
public static class GetProductsQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetProductsQuery"/> with default limit.
    /// </summary>
    /// <param name="page">The current page.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="categories">The categories the products should have.</param>
    /// <returns>A new instance of the <see cref="GetProductsQuery"/> class.</returns>
    public static GetProductsQuery CreateQuery(
        int? page = null,
        int? pageSize = null,
        IEnumerable<string>? categories = null
    )
    {
        return new GetProductsQuery(page, pageSize, categories);
    }
}
