using Application.Categories.Commands.UpdateCategory;

using Domain.UnitTests.TestUtils;

using Bogus;

namespace Application.UnitTests.Categories.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdateCategoryCommand"/> command.
/// </summary>
public static class UpdateCategoryCommandUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="UpdateCategoryCommand"/> class.
    /// </summary>
    /// <param name="id">The category id.</param>
    /// <param name="name">The new category name.</param>
    /// <returns>A new instance of the <see cref="UpdateCategoryCommand"/> class.</returns>
    public static UpdateCategoryCommand CreateCommand(
        string? id = null,
        string? name = null
    )
    {
        return new UpdateCategoryCommand(
            id ?? NumberUtils.CreateRandomLongAsString(),
            name ?? _faker.Commerce.Categories(1).First()
        );
    }
}
