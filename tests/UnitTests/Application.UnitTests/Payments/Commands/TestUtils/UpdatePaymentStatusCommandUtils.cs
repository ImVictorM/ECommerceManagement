using Application.Payments.Commands.UpdatePaymentStatus;

using Domain.PaymentAggregate.Enumerations;
using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Payments.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdatePaymentStatusCommand"/> class.
/// </summary>
public static class UpdatePaymentStatusCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="UpdatePaymentStatusCommand"/> class.
    /// </summary>
    /// <param name="paymentId">The payment identifier.</param>
    /// <param name="status">The payment status.</param>
    /// <returns>
    /// A new instance of the <see cref="UpdatePaymentStatusCommand"/> class.
    /// </returns>
    public static UpdatePaymentStatusCommand CreateCommand(
        string? paymentId = null,
        string? status = null
    )
    {
        return new UpdatePaymentStatusCommand(
            paymentId ?? NumberUtils.CreateRandomLongAsString(),
            status ?? PaymentStatus.Pending.Name
        );
    }
}
