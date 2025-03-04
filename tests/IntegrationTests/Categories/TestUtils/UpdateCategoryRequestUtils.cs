using Contracts.Categories;

using Domain.UnitTests.TestUtils;

namespace IntegrationTests.Categories.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdateCategoryRequest"/> class.
/// </summary>
public static class UpdateCategoryRequestUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="UpdateCategoryRequest"/> class.
    /// </summary>
    /// <param name="name">The category name.</param>
    /// <returns>
    /// A new instance of the <see cref="UpdateCategoryRequest"/> class.
    /// </returns>
    public static UpdateCategoryRequest CreateRequest(
        string? name = null
    )
    {
        return new UpdateCategoryRequest(
            name ?? CategoryUtils.CreateCategoryName()
        );
    }
}
