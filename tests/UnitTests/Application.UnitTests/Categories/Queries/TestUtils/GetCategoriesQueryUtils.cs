using Application.Categories.Queries.GetCategories;

namespace Application.UnitTests.Categories.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetCategoriesQuery"/> query.
/// </summary>
public static class GetCategoriesQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetCategoriesQuery"/> class.
    /// </summary>
    /// <returns>
    /// A new instance of the <see cref="GetCategoriesQuery"/> class.
    /// </returns>
    public static GetCategoriesQuery CreateQuery()
    {
        return new GetCategoriesQuery();
    }
}
