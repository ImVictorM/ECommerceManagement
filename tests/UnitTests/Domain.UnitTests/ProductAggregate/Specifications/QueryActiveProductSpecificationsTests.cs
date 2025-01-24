using Domain.ProductAggregate.Specifications;
using Domain.UnitTests.TestUtils;
using FluentAssertions;

namespace Domain.UnitTests.ProductAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="QueryActiveProductSpecification"/> class.
/// </summary>
public class QueryActiveProductSpecificationsTests
{
    /// <summary>
    /// Verifies that the specification returns true when the product is active.
    /// </summary>
    [Fact]
    public void QueryActiveProductSpecification_WhenProductIsActive_ReturnsTrue()
    {
        var product = ProductUtils.CreateProduct(active: true);

        var specification = new QueryActiveProductSpecification();

        var result = specification.Criteria.Compile()(product);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the specification returns false when the product is not active.
    /// </summary>
    [Fact]
    public void QueryActiveProductSpecification_WhenProductIsNotActive_ReturnsFalse()
    {
        var product = ProductUtils.CreateProduct(active: false);

        var specification = new QueryActiveProductSpecification();

        var result = specification.Criteria.Compile()(product);

        result.Should().BeFalse();
    }
}
