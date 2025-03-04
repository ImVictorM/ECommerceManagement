using Application.ProductFeedback.Queries.GetProductFeedback;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.ProductFeedback.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetProductFeedbackQuery"/> class.
/// </summary>
public static class GetProductFeedbackQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetProductFeedbackQuery"/>
    /// class.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <returns>
    /// A new instance of the <see cref="GetProductFeedbackQuery"/> class.
    /// </returns>
    public static GetProductFeedbackQuery CreateQuery(
        string? productId = null
    )
    {
        return new GetProductFeedbackQuery(
            productId ?? NumberUtils.CreateRandomLongAsString()
        );
    }
}
