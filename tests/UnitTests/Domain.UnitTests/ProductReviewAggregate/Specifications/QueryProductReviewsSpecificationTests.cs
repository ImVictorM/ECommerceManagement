using Domain.ProductAggregate.ValueObjects;
using Domain.ProductReviewAggregate.Specifications;
using Domain.UnitTests.TestUtils;

using FluentAssertions;

namespace Domain.UnitTests.ProductReviewAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="QueryProductReviewsSpecification"/> specification.
/// </summary>
public class QueryProductReviewsSpecificationTests
{
    /// <summary>
    /// Verifies the specification returns true when the review's product identifier
    /// matches.
    /// </summary>
    [Fact]
    public void QueryProductReviews_WithMatchingId_ReturnsTrue()
    {
        var productId = ProductId.Create(1);

        var review = ProductReviewUtils.CreateProductReview(
            productId: productId
        );

        var specification = new QueryProductReviewsSpecification(productId);

        var result = specification.Criteria.Compile()(review);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies the specification returns false when the review's product id
    /// does not match.
    /// </summary>
    [Fact]
    public void QueryProductReviews_WithoutMatchingId_ReturnsFalse()
    {
        var productId = ProductId.Create(1);

        var review = ProductReviewUtils.CreateProductReview(
            productId: ProductId.Create(2)
        );

        var specification = new QueryProductReviewsSpecification(productId);

        var result = specification.Criteria.Compile()(review);

        result.Should().BeFalse();
    }
}
