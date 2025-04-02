using Contracts.ProductReviews;

using Domain.ProductReviewAggregate;

using FluentAssertions;

namespace IntegrationTests.TestUtils.Extensions.ProductReviews;

/// <summary>
/// Extension methods for the <see cref="ProductReviewResponse"/> class.
/// </summary>
public static class ProductReviewResponseExtensions
{
    /// <summary>
    /// Ensures the current response corresponds to the given expected
    /// product review.
    /// </summary>
    /// <param name="response">The current response.</param>
    /// <param name="review">The expected product review.</param>
    public static void EnsureCorrespondsTo(
        this ProductReviewResponse response,
        ProductReview review
    )
    {
        response.Id.Should().Be(review.Id.ToString());
        response.ProductId.Should().Be(review.ProductId.ToString());
        response.Title.Should().Be(review.Title);
        response.Content.Should().Be(review.Content);
        response.StarRating.Should().Be(review.StarRating.Value);
        response.CreatedAt.Should().Be(review.CreatedAt);
        response.UpdatedAt.Should().Be(review.UpdatedAt);
    }

    /// <summary>
    /// Ensures the current response corresponds to the given expected
    /// product review.
    /// </summary>
    /// <param name="response">The current response.</param>
    /// <param name="reviews">
    /// The expected product reviews.
    /// </param>
    public static void EnsureCorrespondsTo(
        this IEnumerable<ProductReviewResponse> response,
        IEnumerable<ProductReview> reviews
    )
    {
        var expectedReviewList = reviews.ToList();
        var responseList = response.ToList();

        responseList.Count.Should().Be(expectedReviewList.Count);

        var expectedReviewsMap = expectedReviewList
            .ToDictionary(f => f.Id.ToString());

        foreach (var responseReview in responseList)
        {
            var currentExpectedProductReview = expectedReviewsMap
                [responseReview.Id];

            responseReview.EnsureCorrespondsTo(
                currentExpectedProductReview
            );
        }
    }
}
