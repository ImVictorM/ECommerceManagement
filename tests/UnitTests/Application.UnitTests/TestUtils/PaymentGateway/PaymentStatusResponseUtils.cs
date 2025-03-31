using Application.Common.PaymentGateway.Responses;

using Domain.PaymentAggregate.Enumerations;

namespace Application.UnitTests.TestUtils.PaymentGateway;

/// <summary>
/// Utilities for the <see cref="PaymentStatusResponse"/> class.
/// </summary>
public static class PaymentStatusResponseUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="PaymentStatusResponse"/> class.
    /// </summary>
    /// <param name="paymentStatus">The current payment status.</param>
    /// <param name="details">The payment status details.</param>
    /// <param name="captured">
    /// A bool value indicating if the payment was captured.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="PaymentStatusResponse"/> class.
    /// </returns>
    public static PaymentStatusResponse CreateResponse(
        PaymentStatus? paymentStatus = null,
        string? details = null,
        bool captured = false
    )
    {
        return new PaymentStatusResponse(
            paymentStatus ?? PaymentStatus.Pending,
            details ?? "Default payment status details",
            captured
        );
    }
}
