using Contracts.ProductFeedback;

using Bogus;

namespace IntegrationTests.ProductFeedback.TestUtils;

/// <summary>
/// Utilities for the <see cref="LeaveProductFeedbackRequest"/> class.
/// </summary>
public static class LeaveProductFeedbackRequestUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="LeaveProductFeedbackRequest"/> class.
    /// </summary>
    /// <param name="title">The feedback title.</param>
    /// <param name="content">The feedback content.</param>
    /// <param name="starRating">The feedback star rating.</param>
    /// <returns>A new instance of the <see cref="LeaveProductFeedbackRequest"/> class.</returns>
    public static LeaveProductFeedbackRequest CreateRequest(
        string? title = null,
        string? content = null,
        int? starRating = null
    )
    {
        return new LeaveProductFeedbackRequest(
            title ?? _faker.Commerce.Product(),
            content ?? _faker.Commerce.ProductDescription(),
            starRating ?? _faker.Random.Int(0, 5)
        );
    }
}
