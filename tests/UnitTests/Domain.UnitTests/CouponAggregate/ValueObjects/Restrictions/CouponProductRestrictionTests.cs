using Domain.CategoryAggregate.ValueObjects;
using Domain.CouponAggregate.ValueObjects;
using Domain.CouponAggregate.ValueObjects.Restrictions;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using SharedKernel.Errors;

using FluentAssertions;

namespace Domain.UnitTests.CouponAggregate.ValueObjects.Restrictions;

/// <summary>
/// Unit tests for the <see cref="CouponProductRestriction"/> class.
/// </summary>
public class CouponProductRestrictionTests
{
    /// <summary>
    /// Verifies that creating a <see cref="CouponProductRestriction"/> with valid
    /// products succeeds.
    /// </summary>
    [Fact]
    public void Create_WithValidProducts_CreatesSuccessfully()
    {
        var productsAllowed = new List<CouponProduct>
        {
            CouponProduct.Create(ProductId.Create(1)),
            CouponProduct.Create(ProductId.Create(2))
        };

        var restriction = CouponProductRestriction.Create(productsAllowed);

        restriction.ProductsAllowed
            .Should().BeEquivalentTo(productsAllowed);
    }

    /// <summary>
    /// Verifies that creating a <see cref="CouponProductRestriction"/> without
    /// allowed products throws an exception.
    /// </summary>
    [Fact]
    public void Create_WithoutProducts_ThrowsEmptyArgumentException()
    {
        FluentActions
            .Invoking(() => CouponProductRestriction.Create([]))
            .Should()
            .Throw<EmptyArgumentException>()
            .WithMessage("Restriction must contain at least one product");
    }

    /// <summary>
    /// Verifies that an order passes the restriction if it contains at least one
    /// allowed product.
    /// </summary>
    [Fact]
    public void PassRestriction_WithAllowedProduct_ReturnsTrue()
    {
        var productId = ProductId.Create(1);
        var allowedProduct = CouponProduct.Create(productId);

        var restriction = CouponProductRestriction.Create([allowedProduct]);

        var order = CouponUtils.CreateCouponOrder(products:
        [
            CouponOrderProduct.Create(
                productId,
                new HashSet<CategoryId>() { CategoryId.Create(2) }
            )
        ]);

        restriction.PassRestriction(order).Should().BeTrue();
    }

    /// <summary>
    /// Verifies that an order does not pass the restriction if it does not contain
    /// any allowed products.
    /// </summary>
    [Fact]
    public void PassRestriction_WithoutAllowedProduct_ReturnsFalse()
    {
        var allowedProduct = CouponProduct.Create(ProductId.Create(1));

        var restriction = CouponProductRestriction.Create([allowedProduct]);

        var order = CouponUtils.CreateCouponOrder(products:
        [
            CouponOrderProduct.Create(
                ProductId.Create(2),
                new HashSet<CategoryId>() { CategoryId.Create(2) }
            )
        ]);

        restriction.PassRestriction(order).Should().BeFalse();
    }
}
