using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using FluentAssertions;

namespace Domain.UnitTests.ProductAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="QueryActiveProductByIdSpecification"/> class.
/// </summary>
public class QueryActiveProductByIdSpecificationTests
{
    /// <summary>
    /// Verifies that the specification returns true when the product is active and the ID matches.
    /// </summary>
    [Fact]
    public void QueryActiveProductByIdSpecification_WhenProductIsActiveAndIdMatches_ReturnsTrue()
    {
        var productId = ProductId.Create(1);
        var product = ProductUtils.CreateProduct(productId, active: true);

        var specification = new QueryActiveProductByIdSpecification(productId);

        var result = specification.Criteria.Compile()(product);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the specification returns false when the product is not active.
    /// </summary>
    [Fact]
    public void QueryActiveProductByIdSpecification_WhenProductIsNotActive_ReturnsFalse()
    {
        var productId = ProductId.Create(1);
        var product = ProductUtils.CreateProduct(productId, active: false);

        var specification = new QueryActiveProductByIdSpecification(productId);

        var result = specification.Criteria.Compile()(product);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the specification returns false when the ID does not match the product's ID.
    /// </summary>
    [Fact]
    public void QueryActiveProductByIdSpecification_WhenIdDoesNotMatch_ReturnsFalse()
    {
        var productId = ProductId.Create(1);
        var product = ProductUtils.CreateProduct(ProductId.Create(2), active: true);

        var specification = new QueryActiveProductByIdSpecification(productId);

        var result = specification.Criteria.Compile()(product);

        result.Should().BeFalse();
    }
}
