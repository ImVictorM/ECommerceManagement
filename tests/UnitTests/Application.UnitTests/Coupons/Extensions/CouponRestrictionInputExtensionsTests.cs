using Application.Coupons.Abstracts;
using Application.Coupons.DTOs;
using Application.Coupons.Errors;
using Application.Coupons.Extensions;

using Domain.CouponAggregate.ValueObjects.Restrictions;

using FluentAssertions;

using Moq;

namespace Application.UnitTests.Coupons.Extensions;

/// <summary>
/// Unit tests for the <see cref="CouponRestrictionInputExtensions"/>
/// class.
/// </summary>
public class CouponRestrictionInputExtensionsTests
{
    /// <summary>
    /// Verifies that parsing a valid <see cref="CategoryRestrictionInput"/>
    /// correctly converts it to a <see cref="CategoryRestriction"/>.
    /// </summary>
    [Fact]
    public void ParseRestriction_WithValidCategoryRestrictionInput_ParsesCorrectly()
    {
        var input = new CategoryRestrictionInput(
            ["1", "2"],
            ["40"]
        );

        var restriction = input.ParseRestriction();
        var categoryRestriction = (CategoryRestriction)restriction;

        restriction.Should().BeOfType<CategoryRestriction>();

        categoryRestriction.CategoriesAllowed
            .Select(c => c.CategoryId.ToString())
            .Should()
            .BeEquivalentTo(input.CategoryAllowedIds);

        categoryRestriction.ProductsFromCategoryNotAllowed
            .Select(p => p.ProductId.ToString())
            .Should()
            .BeEquivalentTo(input.ProductFromCategoryNotAllowedIds);
    }

    /// <summary>
    /// Verifies that parsing a valid <see cref="ProductRestrictionInput"/>
    /// correctly converts it to a <see cref="ProductRestriction"/>.
    /// </summary>
    [Fact]
    public void ParseRestriction_WithValidProductRestrictionInput_ParsesCorrectly()
    {
        var input = new ProductRestrictionInput(
            ["10", "20"]
        );

        var restriction = input.ParseRestriction();
        var productRestriction = (ProductRestriction)restriction;

        restriction.Should().BeOfType<ProductRestriction>();

        productRestriction.ProductsAllowed
            .Select(p => p.ProductId.ToString())
            .Should()
            .BeEquivalentTo(input.ProductAllowedIds);
    }

    /// <summary>
    /// Verifies that parsing an unsupported restriction input
    /// throws a <see cref="NotSupportedRestrictionTypeException"/>.
    /// </summary>
    [Fact]
    public void ParseRestriction_WithUnsupportedInput_ThrowsError()
    {
        var unsupportedInput = new Mock<ICouponRestrictionInput>().Object;

        FluentActions
            .Invoking(unsupportedInput.ParseRestriction)
            .Should()
            .Throw<NotSupportedRestrictionTypeException>();
    }

    /// <summary>
    /// Verifies that parsing a null restriction collection
    /// returns an empty collection.
    /// </summary>
    [Fact]
    public void ParseRestrictions_WithNullInput_ReturnsEmptyCollection()
    {
        IEnumerable<ICouponRestrictionInput>? inputs = null;

        var restrictions = inputs.ParseRestrictions();

        restrictions.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies that parsing a collection of valid restrictions
    /// correctly converts them to their respective domain objects.
    /// </summary>
    [Fact]
    public void ParseRestrictions_WithValidInputs_ParsesCorrectly()
    {
        var inputs = new List<ICouponRestrictionInput>
        {
            new CategoryRestrictionInput(
                ["1"],
                ["100"]
            ),
            new ProductRestrictionInput(
                ["200", "300"]
            )
        };

        var restrictions = inputs.ParseRestrictions().ToList();

        restrictions.Should().HaveCount(2);
        restrictions[0].Should().BeOfType<CategoryRestriction>();
        restrictions[1].Should().BeOfType<ProductRestriction>();
    }
}
