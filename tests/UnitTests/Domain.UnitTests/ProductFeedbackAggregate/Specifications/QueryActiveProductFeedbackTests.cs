using Domain.ProductFeedbackAggregate.Specifications;
using Domain.UnitTests.TestUtils;

using FluentAssertions;

namespace Domain.UnitTests.ProductFeedbackAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="QueryActiveProductFeedback"/> class.
/// </summary>
public class QueryActiveProductFeedbackTests
{
    /// <summary>
    /// Verifies the specification returns true when the product feedback is active.
    /// </summary>
    [Fact]
    public void QueryActiveProductFeedback_WhenFeedbackIsActive_ReturnsTrue()
    {
        var feedback = ProductFeedbackUtils.CreateProductFeedback(active: true);

        var specification = new QueryActiveProductFeedback();

        var result = specification.Criteria.Compile()(feedback);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the specification returns false when the product feedback
    /// is not active.
    /// </summary>
    [Fact]
    public void QueryActiveProductFeedback_WhenFeedbackIsNotActive_ReturnsFalse()
    {
        var feedback = ProductFeedbackUtils.CreateProductFeedback(active: false);

        var specification = new QueryActiveProductFeedback();

        var result = specification.Criteria.Compile()(feedback);

        result.Should().BeFalse();
    }
}
