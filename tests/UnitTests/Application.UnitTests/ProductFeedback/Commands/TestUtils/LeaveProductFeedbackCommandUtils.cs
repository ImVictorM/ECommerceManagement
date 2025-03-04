using Application.ProductFeedback.Commands.LeaveProductFeedback;

using Domain.UnitTests.TestUtils;

using Bogus;

namespace Application.UnitTests.ProductFeedback.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="LeaveProductFeedbackCommand"/> class.
/// </summary>
public static class LeaveProductFeedbackCommandUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="LeaveProductFeedbackCommand"/> class.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <param name="title">The feedback title.</param>
    /// <param name="content">The feedback content.</param>
    /// <param name="starRating">The star rating.</param>
    /// <returns>A new instance of the <see cref="LeaveProductFeedbackCommand"/> class.</returns>
    public static LeaveProductFeedbackCommand CreateCommand(
        string? productId = null,
        string? title = null,
        string? content = null,
        int? starRating = null
    )
    {
        return new LeaveProductFeedbackCommand(
            productId ?? NumberUtils.CreateRandomLongAsString(),
            title ?? _faker.Commerce.Product(),
            content ?? _faker.Commerce.ProductDescription(),
            starRating ?? _faker.Random.Int(0, 5)
        );
    }
}
