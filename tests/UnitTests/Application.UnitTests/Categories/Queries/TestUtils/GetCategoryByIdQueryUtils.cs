using Application.Categories.Queries.GetCategoryById;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Categories.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetCategoryByIdQuery"/> query.
/// </summary>
public static class GetCategoryByIdQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetCategoryByIdQuery"/> class.
    /// </summary>
    /// <param name="id">The category id.</param>
    /// <returns>
    /// A new instance of the <see cref="GetCategoryByIdQuery"/> class.
    /// </returns>
    public static GetCategoryByIdQuery CreateQuery(
        string? id = null
    )
    {
        return new GetCategoryByIdQuery(
            id ?? NumberUtils.CreateRandomLongAsString()
        );
    }
}
