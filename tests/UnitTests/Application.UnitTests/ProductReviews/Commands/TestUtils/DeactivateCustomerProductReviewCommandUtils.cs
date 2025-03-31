using Application.ProductReviews.Commands.DeactivateCustomerProductReview;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.ProductReviews.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="DeactivateCustomerProductReviewCommand"/>
/// class.
/// </summary>
public static class DeactivateCustomerProductReviewCommandUtils
{
    /// <summary>
    /// Creates a new instance of the
    /// <see cref="DeactivateCustomerProductReviewCommand"/> class.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="reviewId">The review identifier.</param>
    /// <returns>
    /// A new instance of the
    /// <see cref="DeactivateCustomerProductReviewCommand"/> class.
    /// </returns>
    public static DeactivateCustomerProductReviewCommand CreateCommand(
        string? userId = null,
        string? reviewId = null
    )
    {
        return new DeactivateCustomerProductReviewCommand(
            userId ?? NumberUtils.CreateRandomLongAsString(),
            reviewId ?? NumberUtils.CreateRandomLongAsString()
        );
    }
}
