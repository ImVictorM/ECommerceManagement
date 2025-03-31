using Application.ProductReviews.Queries.GetProductReviews;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.ProductReviews.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetProductReviewsQuery"/> class.
/// </summary>
public static class GetProductReviewsQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetProductReviewsQuery"/>
    /// class.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <returns>
    /// A new instance of the <see cref="GetProductReviewsQuery"/> class.
    /// </returns>
    public static GetProductReviewsQuery CreateQuery(
        string? productId = null
    )
    {
        return new GetProductReviewsQuery(
            productId ?? NumberUtils.CreateRandomLongAsString()
        );
    }
}
