using Application.Categories.Commands.UpdateCategory;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Categories.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdateCategoryCommand"/> command.
/// </summary>
public static class UpdateCategoryCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="UpdateCategoryCommand"/> class.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    /// <param name="name">The new category name.</param>
    /// <returns>
    /// A new instance of the <see cref="UpdateCategoryCommand"/> class.
    /// </returns>
    public static UpdateCategoryCommand CreateCommand(
        string? id = null,
        string? name = null
    )
    {
        return new UpdateCategoryCommand(
            id ?? NumberUtils.CreateRandomLongAsString(),
            name ?? CategoryUtils.CreateCategoryName()
        );
    }
}
