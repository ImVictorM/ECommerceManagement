using Application.Categories.Commands.CreateCategory;
using Bogus;

namespace Application.UnitTests.Categories.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="CreateCategoryCommand"/> command;
/// </summary>
public static class CreateCategoryCommandUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="CreateCategoryCommand"/> class.
    /// </summary>
    /// <param name="name">The category name.</param>
    /// <returns>A new instance of the <see cref="CreateCategoryCommand"/> class.</returns>
    public static CreateCategoryCommand CreateCommand(
        string? name = null
    )
    {
        return new CreateCategoryCommand(name ?? _faker.Commerce.Categories(1).First());
    }
}
