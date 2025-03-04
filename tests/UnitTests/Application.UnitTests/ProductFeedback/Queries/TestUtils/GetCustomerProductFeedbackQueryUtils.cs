using Application.ProductFeedback.Queries.GetCustomerProductFeedback;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.ProductFeedback.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetCustomerProductFeedbackQuery"/> class.
/// </summary>
public static class GetCustomerProductFeedbackQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetCustomerProductFeedbackQuery"/>
    /// class.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <returns>
    /// A new instance of the <see cref="GetCustomerProductFeedbackQuery"/> class.
    /// </returns>
    public static GetCustomerProductFeedbackQuery CreateQuery(
        string? userId = null
    )
    {
        return new GetCustomerProductFeedbackQuery(
            userId ?? NumberUtils.CreateRandomLongAsString()
        );
    }
}
