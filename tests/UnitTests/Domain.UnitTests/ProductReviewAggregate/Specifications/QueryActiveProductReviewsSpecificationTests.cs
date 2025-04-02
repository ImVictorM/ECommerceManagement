using Domain.ProductReviewAggregate.Specifications;
using Domain.UnitTests.TestUtils;

using FluentAssertions;

namespace Domain.UnitTests.ProductReviewAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="QueryActiveProductReviewsSpecification"/>
/// specification.
/// </summary>
public class QueryActiveProductReviewsSpecificationTests
{
    /// <summary>
    /// Verifies the specification returns true when the product review is active.
    /// </summary>
    [Fact]
    public void QueryActiveProductReviews_WithActiveReview_ReturnsTrue()
    {
        var review = ProductReviewUtils.CreateProductReview(active: true);

        var specification = new QueryActiveProductReviewsSpecification();

        var result = specification.Criteria.Compile()(review);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the specification returns false when the product review
    /// is not active.
    /// </summary>
    [Fact]
    public void QueryActiveProductReviews_WithInactiveReview_ReturnsFalse()
    {
        var review = ProductReviewUtils.CreateProductReview(active: false);

        var specification = new QueryActiveProductReviewsSpecification();

        var result = specification.Criteria.Compile()(review);

        result.Should().BeFalse();
    }
}
