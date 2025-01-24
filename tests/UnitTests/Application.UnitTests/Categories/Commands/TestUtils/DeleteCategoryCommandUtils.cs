using Application.Categories.Commands.DeleteCategory;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Categories.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="DeleteCategoryCommand"/> command.
/// </summary>
public static class DeleteCategoryCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="DeleteCategoryCommand"/> class.
    /// </summary>
    /// <param name="id">The category id.</param>
    /// <returns>A new instance of the <see cref="DeleteCategoryCommand"/> class.</returns>
    public static DeleteCategoryCommand CreateCommand(string? id = null)
    {
        return new DeleteCategoryCommand(id ?? NumberUtils.CreateRandomLongAsString());
    }
}
