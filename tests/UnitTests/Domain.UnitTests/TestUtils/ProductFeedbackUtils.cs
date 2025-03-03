using Domain.ProductAggregate.ValueObjects;
using Domain.ProductFeedbackAggregate;
using Domain.ProductFeedbackAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.UnitTests.TestUtils.Extensions;

using Bogus;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="ProductFeedback"/> class.
/// </summary>
public static class ProductFeedbackUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="ProductFeedback"/> class.
    /// </summary>
    /// <param name="id">The feedback id.</param>
    /// <param name="userId">The user id.</param>
    /// <param name="productId">The product id.</param>
    /// <param name="title">The feedback title.</param>
    /// <param name="content">The feedback content.</param>
    /// <param name="starRating">The feedback star rating.</param>
    /// <param name="active">
    /// A boolean value indicating if the product feedback should be active or not.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="ProductFeedback"/> class.
    /// </returns>
    public static ProductFeedback CreateProductFeedback(
        ProductFeedbackId? id = null,
        UserId? userId = null,
        ProductId? productId = null,
        string? title = null,
        string? content = null,
        StarRating? starRating = null,
        bool active = true
    )
    {
        var feedback = ProductFeedback.Create(
            userId ?? UserId.Create(_faker.Random.Long()),
            productId ?? ProductId.Create(_faker.Random.Long()),
            title ?? _faker.Commerce.Product(),
            content ?? _faker.Commerce.ProductDescription(),
            starRating ?? StarRating.Create(_faker.Random.Int(0, 5))
        );

        if (!active)
        {
            feedback.Deactivate();
        }

        if (id != null)
        {
            feedback.SetIdUsingReflection(id);
        }

        return feedback;
    }
}
