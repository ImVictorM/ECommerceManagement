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
    /// Verifies the category is created correctly with different valid names.
    /// </summary>
    /// <param name="inputName">The category name input.</param>
    /// <param name="expectedName">The expected category name.</param>
    [Theory]
    [InlineData("BookingStationery", "booking_stationery")]
    [InlineData("TECH", "tech")]
    public void Create_WithDifferentValidNames_ReturnsCategoryWithExpectedName(
        string inputName,
        string expectedName
    )
    {
        var category = CategoryUtils.CreateCategory(name: inputName);

        category.Name.Should().Be(expectedName);
    }

    /// <summary>
    /// Verifies it is possible to update the category name.
    /// </summary>
    [Fact]
    public void Update_WithValidInput_UpdatesTheCategoryName()
    {
        var newCategoryName = "TECHNOLOGY";

        var category = CategoryUtils.CreateCategory(name: "old_tech_category");

        category.Update(newCategoryName);

        category.Name.Should().Be("technology");
    }
}
