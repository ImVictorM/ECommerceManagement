using Application.ProductReviews.Queries.GetCustomerProductReviews;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.ProductReviews.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetCustomerProductReviewsQuery"/> class.
/// </summary>
public static class GetCustomerProductReviewsQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetCustomerProductReviewsQuery"/>
    /// class.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>
    /// A new instance of the <see cref="GetCustomerProductReviewsQuery"/> class.
    /// </returns>
    public static GetCustomerProductReviewsQuery CreateQuery(
        string? userId = null
    )
    {
        return new GetCustomerProductReviewsQuery(
            userId ?? NumberUtils.CreateRandomLongAsString()
        );
    }
}
