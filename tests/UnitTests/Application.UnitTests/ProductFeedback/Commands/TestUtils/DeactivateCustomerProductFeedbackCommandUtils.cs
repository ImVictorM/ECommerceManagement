using Application.ProductFeedback.Commands.DeactivateCustomerProductFeedback;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.ProductFeedback.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="DeactivateCustomerProductFeedbackCommand"/>
/// class.
/// </summary>
public static class DeactivateCustomerProductFeedbackCommandUtils
{
    /// <summary>
    /// Creates a new instance of the
    /// <see cref="DeactivateCustomerProductFeedbackCommand"/> class.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="feedbackId">The feedback item id.</param>
    /// <returns>
    /// A new instance of the
    /// <see cref="DeactivateCustomerProductFeedbackCommand"/> class.
    /// </returns>
    public static DeactivateCustomerProductFeedbackCommand CreateCommand(
        string? userId = null,
        string? feedbackId = null
    )
    {
        return new DeactivateCustomerProductFeedbackCommand(
            userId ?? NumberUtils.CreateRandomLongAsString(),
            feedbackId ?? NumberUtils.CreateRandomLongAsString()
        );
    }
}
