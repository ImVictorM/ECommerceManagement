using Application.Categories.Commands.CreateCategory;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Categories.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="CreateCategoryCommand"/> command.
/// </summary>
public static class CreateCategoryCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="CreateCategoryCommand"/> class.
    /// </summary>
    /// <param name="name">The category name.</param>
    /// <returns>
    /// A new instance of the <see cref="CreateCategoryCommand"/> class.
    /// </returns>
    public static CreateCategoryCommand CreateCommand(
        string? name = null
    )
    {
        return new CreateCategoryCommand(
            name ?? CategoryUtils.CreateCategoryName()
        );
    }
}
