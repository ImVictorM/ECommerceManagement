using Application.ProductReviews.DTOs.Results;

using Domain.ProductReviewAggregate;
using Domain.ProductReviewAggregate.ValueObjects;

using FluentAssertions;

namespace Application.UnitTests.ProductReviews.TestUtils.Extensions;

/// <summary>
/// Utilities for the <see cref="ProductReviewResult"/> class.
/// </summary>
public static class ProductReviewResultExtensions
{
    /// <summary>
    /// Ensures an <see cref="ProductReviewResult"/> matches the results
    /// from an <see cref="ProductReview"/> review.
    /// </summary>
    /// <param name="result">The current result.</param>
    /// <param name="review">The review to be compared.</param>
    public static void EnsureCorrespondsTo(
        this ProductReviewResult result,
        ProductReview review
    )
    {
        result.Id.Should().Be(review.Id.ToString());
        result.ProductId.Should().Be(review.ProductId.ToString());
        result.Title.Should().Be(review.Title);
        result.Content.Should().Be(review.Content);
        result.StarRating.Should().Be(review.StarRating.Value);
        result.CreatedAt.Should().Be(review.CreatedAt);
        result.UpdatedAt.Should().Be(review.UpdatedAt);
    }

    /// <summary>
    /// Ensures a collection of <see cref="ProductReviewResult"/> matches the results
    /// from a collection of <see cref="ProductReview"/>.
    /// </summary>
    /// <param name="results">The current results.</param>
    /// <param name="reviews">The reviews to be compared.</param>
    public static void EnsureCorrespondsTo(
        this IEnumerable<ProductReviewResult> results,
        IEnumerable<ProductReview> reviews
    )
    {
        foreach (var result in results)
        {
            var review = reviews
                .FirstOrDefault(p => p.Id == ProductReviewId.Create(result.Id));

            review.Should().NotBeNull();
            result.EnsureCorrespondsTo(review!);
        }
    }
}
