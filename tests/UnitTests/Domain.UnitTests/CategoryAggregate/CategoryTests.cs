using Domain.UnitTests.TestUtils;
using Domain.CategoryAggregate;
using FluentAssertions;

namespace Domain.UnitTests.CategoryAggregate;

/// <summary>
/// Unit tests for the <see cref="Category"/> class.
/// </summary>
public class CategoryTests
{
    /// <summary>
    /// Tests the category is created correctly with different valid names.
    /// </summary>
    /// <param name="inputName">The category name input.</param>
    /// <param name="expectedName">The expected category name.</param>
    [Theory]
    [InlineData("CATEGORY_1", "category_1")]
    [InlineData("Category2", "category_2")]
    public void CreateCategory_WithDifferentValidNames_ReturnsCategoryWithExpectedName(string inputName, string expectedName)
    {
        var category = CategoryUtils.CreateCategory(inputName);

        category.Name.Should().Be(expectedName);
    }
}
