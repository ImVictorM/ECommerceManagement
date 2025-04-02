using Application.ProductReviews.Queries.Projections;

using Domain.ProductReviewAggregate;
using Domain.UserAggregate;

namespace Application.UnitTests.ProductReviews.TestUtils.Projections;

/// <summary>
/// Utilities for the <see cref="ProductReviewDetailedProjection"/> class.
/// </summary>
public static class ProductReviewDetailedProjectionUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="ProductReviewDetailedProjection"/>
    /// class.
    /// </summary>
    /// <param name="review">The review representing the projection.</param>
    /// <param name="user">The user who posted the review.</param>
    /// <returns>
    /// A new instance of the <see cref="ProductReviewDetailedProjection"/> class.
    /// </returns>
    public static ProductReviewDetailedProjection CreateProjection(
        ProductReview review,
        User user
    )
    {
        return new ProductReviewDetailedProjection(
            review.Id,
            review.ProductId,
            review.Title,
            review.Content,
            review.StarRating,
            review.CreatedAt,
            review.UpdatedAt,
            new ProductReviewUserProjection(user.Id, user.Name)
        );
    }
}
