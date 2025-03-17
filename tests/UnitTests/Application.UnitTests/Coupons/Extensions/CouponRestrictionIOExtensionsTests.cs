using Application.Coupons.DTOs.Restrictions;
using Application.Coupons.Errors;
using Application.Coupons.Extensions;

using Domain.CategoryAggregate.ValueObjects;
using Domain.CouponAggregate.Abstracts;
using Domain.CouponAggregate.ValueObjects;
using Domain.CouponAggregate.ValueObjects.Restrictions;
using Domain.ProductAggregate.ValueObjects;

using FluentAssertions;
using Moq;

namespace Application.UnitTests.Coupons.Extensions;

/// <summary>
/// Unit tests for the <see cref="CouponRestrictionIOExtensions"/>
/// class.
/// </summary>
public class CouponRestrictionIOExtensionsTests
{
    /// <summary>
    /// Verifies that parsing a valid <see cref="CouponCategoryRestrictionIO"/>
    /// correctly converts it to a <see cref="CouponCategoryRestriction"/>.
    /// </summary>
    [Fact]
    public void ParseRestriction_WhenInputIsValidCategoryRestrictionIO_ParsesCorrectlyToDomain()
    {
        var restrictionIO = new CouponCategoryRestrictionIO(
            ["1", "2"],
            ["40"]
        );

        var restrictionParsed = restrictionIO.ParseRestriction();
        var categoryRestriction = (CouponCategoryRestriction)restrictionParsed;

        restrictionParsed.Should().BeOfType<CouponCategoryRestriction>();

        categoryRestriction.CategoriesAllowed
            .Select(c => c.CategoryId.ToString())
            .Should()
            .BeEquivalentTo(restrictionIO.CategoryAllowedIds);

        categoryRestriction.ProductsFromCategoryNotAllowed
            .Select(p => p.ProductId.ToString())
            .Should()
            .BeEquivalentTo(restrictionIO.ProductFromCategoryNotAllowedIds);
    }

    /// <summary>
    /// Verifies that parsing a valid <see cref="CouponProductRestrictionIO"/>
    /// correctly converts it to a <see cref="CouponProductRestriction"/>.
    /// </summary>
    [Fact]
    public void ParseRestriction_WhenInputIsValidProductRestrictionIO_ParsesCorrectlyToDomain()
    {
        var input = new CouponProductRestrictionIO(
            ["10", "20"]
        );

        var restriction = input.ParseRestriction();
        var productRestriction = (CouponProductRestriction)restriction;

        restriction.Should().BeOfType<CouponProductRestriction>();

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
    public void ParseRestriction_WhenInputIsUnsupportedIO_ThrowsError()
    {
        var unsupportedInput = new Mock<CouponRestrictionIO>().Object;

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
        IEnumerable<CouponRestrictionIO>? inputs = null;

        var restrictions = inputs.ParseRestrictions();

        restrictions.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies that parsing a collection of valid restrictions
    /// correctly converts them to their respective domain objects.
    /// </summary>
    [Fact]
    public void ParseRestrictions_WithValidCollectionInput_ParsesCorrectlyToDomain()
    {
        var inputs = new List<CouponRestrictionIO>
        {
            new CouponCategoryRestrictionIO(
                ["1"],
                ["100"]
            ),
            new CouponProductRestrictionIO(
                ["200", "300"]
            )
        };

        var restrictions = inputs.ParseRestrictions().ToList();

        restrictions.Should().HaveCount(2);
        restrictions[0].Should().BeOfType<CouponCategoryRestriction>();
        restrictions[1].Should().BeOfType<CouponProductRestriction>();
    }

    /// <summary>
    /// Verifies that parsing a valid <see cref="CouponCategoryRestriction"/>
    /// correctly converts it to a <see cref="CouponCategoryRestrictionIO"/>.
    /// </summary>
    [Fact]
    public void ParseRestriction_WithValidCategoryRestriction_ParsesCorrectlyToIO()
    {
        var restriction = CouponCategoryRestriction.Create(
            categoriesAllowed:
            [
                CouponCategory.Create(CategoryId.Create("1"))
            ],
            productsFromCategoryNotAllowed:
            [
                CouponProduct.Create(ProductId.Create("40"))
            ]
        );

        var result = restriction.ParseRestriction();
        var categoryIO = (CouponCategoryRestrictionIO)result;

        result.Should().BeOfType<CouponCategoryRestrictionIO>();
        categoryIO.CategoryAllowedIds.Should().BeEquivalentTo(["1"]);
        categoryIO.ProductFromCategoryNotAllowedIds.Should().BeEquivalentTo(["40"]);
    }

    /// <summary>
    /// Verifies that parsing a valid <see cref="CouponProductRestriction"/>
    /// correctly converts it to a <see cref="CouponProductRestrictionIO"/>.
    /// </summary>
    [Fact]
    public void ParseRestriction_WithValidProductRestriction_ParsesCorrectlyToIO()
    {
        var restriction = CouponProductRestriction.Create(
            productsAllowed:
            [
                CouponProduct.Create(ProductId.Create("10"))
            ]
        );

        var result = restriction.ParseRestriction();
        var productIO = (CouponProductRestrictionIO)result;

        result.Should().BeOfType<CouponProductRestrictionIO>();
        productIO.ProductAllowedIds.Should().BeEquivalentTo(["10"]);
    }

    /// <summary>
    /// Verifies that parsing an unsupported restriction type
    /// throws a <see cref="NotSupportedRestrictionTypeException"/>.
    /// </summary>
    [Fact]
    public void ParseRestriction_WithUnsupportedRestriction_ThrowsError()
    {
        var unsupportedRestriction = new Mock<CouponRestriction>().Object;

        FluentActions
            .Invoking(unsupportedRestriction.ParseRestriction)
            .Should()
            .Throw<NotSupportedRestrictionTypeException>();
    }

    /// <summary>
    /// Verifies that parsing a collection of valid restrictions
    /// correctly converts them to their respective input objects.
    /// </summary>
    [Fact]
    public void ParseRestrictions_WithValidRestrictions_ParsesCorrectlyToIO()
    {
        var restrictions = new List<CouponRestriction>
        {
            CouponCategoryRestriction.Create(
                [CouponCategory.Create(CategoryId.Create("1"))],
                [CouponProduct.Create(ProductId.Create("40"))]
            ),
            CouponProductRestriction.Create(
                [CouponProduct.Create(ProductId.Create("10"))]
            )
        };

        var result = restrictions.ParseRestrictions().ToList();

        result.Should().HaveCount(2);
        result[0].Should().BeOfType<CouponCategoryRestrictionIO>();
        result[1].Should().BeOfType<CouponProductRestrictionIO>();
    }
}
