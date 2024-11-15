using FluentAssertions;
using Moq;
using SharedKernel.Interfaces;
using SharedKernel.Specifications;

namespace SharedKernel.UnitTests.Specifications;

/// <summary>
/// Unit tests for the <see cref="DiscountThresholdSpecification"/> specification, 
/// validating its behavior when determining if a discountable entity satisfies the specified threshold.
/// </summary>
public class DiscountThresholdSpecificationTests
{
    private readonly DiscountThresholdSpecification _specification;

    /// <summary>
    /// Initiates a new instance of the <see cref="DiscountThresholdSpecificationTests"/>
    /// </summary>
    public DiscountThresholdSpecificationTests()
    {
        _specification = new DiscountThresholdSpecification();
    }

    /// <summary>
    /// Verifies that the specification is satisfied when the price after discounts is above the threshold.
    /// </summary>
    [Fact]
    public void DiscountThresholdSpecification_WhenPriceAfterDiscountsIsAboveThreshold_IsSatisfiedByShouldReturnTrue()
    {
        var mockDiscountable = new Mock<IDiscountable>();

        mockDiscountable.Setup(d => d.BasePrice).Returns(100m);
        mockDiscountable.Setup(d => d.GetPriceAfterDiscounts()).Returns(15m);

        var result = _specification.IsSatisfiedBy(mockDiscountable.Object);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the specification is not satisfied when the price after discounts is equal to the threshold.
    /// </summary>
    [Fact]
    public void DiscountThresholdSpecification_WhenPriceAfterDiscountsIsEqualToThreshold_IsSatisfiedByShouldReturnFalse()
    {
        var mockDiscountable = new Mock<IDiscountable>();

        mockDiscountable.Setup(d => d.BasePrice).Returns(100m);
        mockDiscountable.Setup(d => d.GetPriceAfterDiscounts()).Returns(10m);

        var result = _specification.IsSatisfiedBy(mockDiscountable.Object);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the specification is not satisfied when the price after discounts is below the threshold.
    /// </summary>
    [Fact]
    public void DiscountThresholdSpecification_WhenPriceAfterDiscountsIsBelowThreshold_IsSatisfiedByShouldReturnFalse()
    {
        var mockDiscountable = new Mock<IDiscountable>();

        mockDiscountable.Setup(d => d.BasePrice).Returns(100m);
        mockDiscountable.Setup(d => d.GetPriceAfterDiscounts()).Returns(5m);

        var result = _specification.IsSatisfiedBy(mockDiscountable.Object);

        result.Should().BeFalse();
    }
}
