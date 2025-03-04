using Domain.ProductFeedbackAggregate.Specifications;
using Domain.UserAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using FluentAssertions;

namespace Domain.UnitTests.ProductFeedbackAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="QueryUserProductFeedback"/> specification.
/// </summary>
public class QueryUserProductFeedbackTests
{
    /// <summary>
    /// Verifies the specification returns true when the feedback's user id
    /// matches.
    /// </summary>
    [Fact]
    public void QueryUserProductFeedback_WhenUserIdMatches_ReturnsTrue()
    {
        var userId = UserId.Create(1);

        var feedback = ProductFeedbackUtils.CreateProductFeedback(
            userId: userId
        );

        var specification = new QueryUserProductFeedback(userId);

        var result = specification.Criteria.Compile()(feedback);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the specification returns false when the feedback's user id
    /// does not match.
    /// </summary>
    [Fact]
    public void QueryUserProductFeedback_WhenUserIdDoesNotMatch_ReturnsFalse()
    {
        var userId = UserId.Create(1);

        var feedback = ProductFeedbackUtils.CreateProductFeedback(
            userId: UserId.Create(2)
        );

        var specification = new QueryUserProductFeedback(userId);

        var result = specification.Criteria.Compile()(feedback);

        result.Should().BeFalse();
    }
}
