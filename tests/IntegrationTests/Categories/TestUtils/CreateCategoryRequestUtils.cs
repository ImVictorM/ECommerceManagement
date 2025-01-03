using Contracts.Categories;
using Domain.UnitTests.TestUtils;

namespace IntegrationTests.Categories.TestUtils;

/// <summary>
/// Utilities for the <see cref="CreateCategoryRequest"/> class.
/// </summary>
public static class CreateCategoryRequestUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="CreateCategoryRequest"/> class.
    /// </summary>
    /// <param name="name">The category name.</param>
    /// <returns>A new instance of the <see cref="CreateCategoryRequest"/> class.</returns>
    public static CreateCategoryRequest CreateRequest(
        string? name = null
    )
    {
        return new CreateCategoryRequest(
            name ?? CategoryUtils.CreateCategoryName()
        );
    }
}
