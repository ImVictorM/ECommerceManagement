using Domain.UserAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.ProductReviewAggregate.Specifications;

using FluentAssertions;

namespace Domain.UnitTests.ProductReviewAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="QueryUserReviewsSpecification"/> specification.
/// </summary>
public class QueryUserReviewsSpecificationTests
{
    /// <summary>
    /// Verifies the specification returns true when the review's user identifier
    /// matches.
    /// </summary>
    [Fact]
    public void QueryUserReviews_WithMatchingUserId_ReturnsTrue()
    {
        var userId = UserId.Create(1);

        var review = ProductReviewUtils.CreateProductReview(
            userId: userId
        );

        var specification = new QueryUserReviewsSpecification(userId);

        var result = specification.Criteria.Compile()(review);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the specification returns false when the review's user identifier
    /// does not match.
    /// </summary>
    [Fact]
    public void QueryUserReviews_WithoutMatchingUserId_ReturnsFalse()
    {
        var userId = UserId.Create(1);

        var review = ProductReviewUtils.CreateProductReview(
            userId: UserId.Create(2)
        );

        var specification = new QueryUserReviewsSpecification(userId);

        var result = specification.Criteria.Compile()(review);

        result.Should().BeFalse();
    }
}
