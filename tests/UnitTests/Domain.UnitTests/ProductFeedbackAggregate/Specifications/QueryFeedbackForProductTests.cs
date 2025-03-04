using Domain.ProductAggregate.ValueObjects;
using Domain.ProductFeedbackAggregate.Specifications;
using Domain.UnitTests.TestUtils;

using FluentAssertions;

namespace Domain.UnitTests.ProductFeedbackAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="QueryFeedbackForProduct"/> specification.
/// </summary>
public class QueryFeedbackForProductTests
{
    /// <summary>
    /// Verifies the specification returns true when the feedback's product id
    /// matches.
    /// </summary>
    [Fact]
    public void QueryFeedbackForProduct_WhenProductIdMatches_ReturnsTrue()
    {
        var productId = ProductId.Create(1);

        var feedback = ProductFeedbackUtils.CreateProductFeedback(
            productId: productId
        );

        var specification = new QueryFeedbackForProduct(productId);

        var result = specification.Criteria.Compile()(feedback);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the specification returns false when the feedback's product id
    /// does not match.
    /// </summary>
    [Fact]
    public void QueryFeedbackForProduct_WhenProductIdDoesNotMatch_ReturnsFalse()
    {
        var productId = ProductId.Create(1);

        var feedback = ProductFeedbackUtils.CreateProductFeedback(
            productId: ProductId.Create(2)
        );

        var specification = new QueryFeedbackForProduct(productId);

        var result = specification.Criteria.Compile()(feedback);

        result.Should().BeFalse();
    }
}
