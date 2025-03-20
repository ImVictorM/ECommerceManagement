using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using FluentAssertions;

namespace Domain.UnitTests.ProductAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="ProductSaleDiscountThresholdSpecification"/>
/// specification.
/// </summary>
public class ProductSaleDiscountThresholdSpecificationTests
{
    /// <summary>
    /// Verifies that the specification is satisfied when the sale price is above
    /// the minimum allowed price.
    /// </summary>
    [Theory]
    [InlineData(100, 10)]
    [InlineData(100, 11)]
    [InlineData(100, 100)]
    [InlineData(100, 50)]
    public void IsSatisfiedBy_WhenSalePriceAboveThreshold_ReturnsTrue(
        decimal basePrice,
        decimal salePrice
    )
    {
        var product = ProductUtils.CreateProduct(
            id: ProductId.Create(1),
            basePrice: basePrice
        );

        var spec = new ProductSaleDiscountThresholdSpecification(salePrice);

        var result = spec.IsSatisfiedBy(product);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the specification is not satisfied when the sale price
    /// falls below the minimum allowed price.
    /// </summary>
    [Theory]
    [InlineData(100, 5)]
    [InlineData(100, 9)]
    [InlineData(100, 1)]
    public void IsSatisfiedBy_WhenSalePriceBelowThreshold_ReturnsFalse(
        decimal basePrice,
        decimal salePrice
    )
    {
        var product = ProductUtils.CreateProduct(
            id: ProductId.Create(1),
            basePrice: basePrice
        );

        var spec = new ProductSaleDiscountThresholdSpecification(salePrice);

        var result = spec.IsSatisfiedBy(product);

        result.Should().BeFalse();
    }
}
