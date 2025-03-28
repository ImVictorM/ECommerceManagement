using Domain.CategoryAggregate.ValueObjects;
using Domain.CouponAggregate.ValueObjects;
using Domain.CouponAggregate.ValueObjects.Restrictions;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using SharedKernel.Errors;

using FluentAssertions;

namespace Domain.UnitTests.CouponAggregate.ValueObjects.Restrictions;

/// <summary>
/// Unit tests for the <see cref="CouponCategoryRestriction"/> class.
/// </summary>
public class CouponCategoryRestrictionTests
{
    /// <summary>
    /// Verifies that creating a <see cref="CouponCategoryRestriction"/> with valid
    /// parameters succeeds.
    /// </summary>
    [Fact]
    public void Create_WithValidCategories_CreatesSuccessfully()
    {
        var categoriesAllowed = new List<CouponCategory>
        {
            CouponCategory.Create(CategoryId.Create(1)),
            CouponCategory.Create(CategoryId.Create(2))
        };
        var productsExcluded = new List<CouponProduct>
        {
            CouponProduct.Create(ProductId.Create(3))
        };

        var restriction = CouponCategoryRestriction.Create(
            categoriesAllowed,
            productsExcluded
        );

        restriction.CategoriesAllowed
            .Should().BeEquivalentTo(categoriesAllowed);

        restriction.ProductsFromCategoryNotAllowed
            .Should().BeEquivalentTo(productsExcluded);
    }

    /// <summary>
    /// Verifies that creating a <see cref="CouponCategoryRestriction"/> without
    /// allowed categories throws an exception.
    /// </summary>
    [Fact]
    public void Create_WithoutCategories_ThrowsEmptyArgumentException()
    {
        FluentActions
            .Invoking(() => CouponCategoryRestriction.Create([]))
            .Should()
            .Throw<EmptyArgumentException>()
            .WithMessage("Restriction must contain at least one category");
    }

    /// <summary>
    /// Verifies that a product passes the restriction if it belongs to
    /// an allowed category.
    /// </summary>
    [Fact]
    public void PassRestriction_WithAllowedProduct_ReturnsTrue()
    {
        var categoryId = CategoryId.Create(1);
        var allowedCategory = CouponCategory.Create(categoryId);

        var restriction = CouponCategoryRestriction.Create([allowedCategory]);

        var order = CouponUtils.CreateCouponOrder(products:
        [
            CouponOrderProduct.Create(
                ProductId.Create(100),
                new HashSet<CategoryId>() { categoryId }
            )
        ]);

        restriction.PassRestriction(order).Should().BeTrue();
    }

    /// <summary>
    /// Verifies that a product does not pass the restriction if it is excluded.
    /// </summary>
    [Fact]
    public void PassRestriction_WithExcludedProduct_ReturnsFalse()
    {
        var categoryId = CategoryId.Create(1);
        var productId = ProductId.Create(100);

        var restriction = CouponCategoryRestriction.Create(
            [CouponCategory.Create(categoryId)],
            [CouponProduct.Create(productId)]
        );

        var order = CouponUtils.CreateCouponOrder(products:
        [
            CouponOrderProduct.Create(
                productId,
                new HashSet<CategoryId>() { categoryId }
            )
        ]);

        restriction.PassRestriction(order).Should().BeFalse();
    }
}
