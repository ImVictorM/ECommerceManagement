using Application.ProductReviews.Commands.LeaveProductReview;

using Domain.UnitTests.TestUtils;

using Bogus;

namespace Application.UnitTests.ProductReviews.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="LeaveProductReviewCommand"/> class.
/// </summary>
public static class LeaveProductReviewCommandUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="LeaveProductReviewCommand"/> class.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="title">The review title.</param>
    /// <param name="content">The review content.</param>
    /// <param name="starRating">The star rating.</param>
    /// <returns>
    /// A new instance of the <see cref="LeaveProductReviewCommand"/> class.
    /// </returns>
    public static LeaveProductReviewCommand CreateCommand(
        string? productId = null,
        string? title = null,
        string? content = null,
        int? starRating = null
    )
    {
        return new LeaveProductReviewCommand(
            productId ?? NumberUtils.CreateRandomLongAsString(),
            title ?? _faker.Commerce.Product(),
            content ?? _faker.Commerce.ProductDescription(),
            starRating ?? _faker.Random.Int(0, 5)
        );
    }
}
