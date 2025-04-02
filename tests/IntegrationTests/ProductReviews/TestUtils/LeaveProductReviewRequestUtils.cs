using Contracts.ProductReviews;

using Bogus;

namespace IntegrationTests.ProductReviews.TestUtils;

/// <summary>
/// Utilities for the <see cref="LeaveProductReviewRequest"/> class.
/// </summary>
public static class LeaveProductReviewRequestUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="LeaveProductReviewRequest"/>
    /// class.
    /// </summary>
    /// <param name="title">The review title.</param>
    /// <param name="content">The review content.</param>
    /// <param name="starRating">The review star rating.</param>
    /// <returns>
    /// A new instance of the <see cref="LeaveProductReviewRequest"/> class.
    /// </returns>
    public static LeaveProductReviewRequest CreateRequest(
        string? title = null,
        string? content = null,
        int? starRating = null
    )
    {
        return new LeaveProductReviewRequest(
            title ?? _faker.Commerce.Product(),
            content ?? _faker.Commerce.ProductDescription(),
            starRating ?? _faker.Random.Int(0, 5)
        );
    }
}
