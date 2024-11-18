using Application.Products.Queries.GetProductCategories;

namespace Application.UnitTests.Products.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetProductCategoriesQuery"/> query.
/// </summary>
public static class GetProductCategoriesQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetProductCategoriesQuery"/> query.
    /// </summary>
    /// <returns>A new instance of the <see cref="GetProductCategoriesQuery"/> query.</returns>
    public static GetProductCategoriesQuery CreateQuery()
    {
        return new GetProductCategoriesQuery();
    }
}
