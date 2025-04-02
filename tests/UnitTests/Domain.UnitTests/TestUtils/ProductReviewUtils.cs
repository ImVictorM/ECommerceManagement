using Domain.ProductAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using Domain.ProductReviewAggregate.ValueObjects;
using Domain.ProductReviewAggregate;

using SharedKernel.UnitTests.TestUtils.Extensions;

using Bogus;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="ProductReview"/> class.
/// </summary>
public static class ProductReviewUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="ProductReview"/> class.
    /// </summary>
    /// <param name="id">The review identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="productId">The product identifier.</param>
    /// <param name="title">The review title.</param>
    /// <param name="content">The review content.</param>
    /// <param name="starRating">The review star rating.</param>
    /// <param name="active">
    /// A boolean value indicating if the product review should be active or not.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="ProductReview"/> class.
    /// </returns>
    public static ProductReview CreateProductReview(
        ProductReviewId? id = null,
        UserId? userId = null,
        ProductId? productId = null,
        string? title = null,
        string? content = null,
        StarRating? starRating = null,
        bool active = true
    )
    {
        var review = ProductReview.Create(
            userId ?? UserId.Create(_faker.Random.Long()),
            productId ?? ProductId.Create(_faker.Random.Long()),
            title ?? _faker.Commerce.Product(),
            content ?? _faker.Commerce.ProductDescription(),
            starRating ?? StarRating.Create(_faker.Random.Int(0, 5))
        );

        if (!active)
        {
            review.Deactivate();
        }

        if (id != null)
        {
            review.SetIdUsingReflection(id);
        }

        return review;
    }
}
