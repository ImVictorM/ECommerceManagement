using Domain.PaymentAggregate.Events;
using Domain.PaymentAggregate;
using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.TestUtils.Events.Payments;

/// <summary>
/// Utilities for the <see cref="PaymentRejected"/> class.
/// </summary>
public static class PaymentRejectedUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="PaymentRejected"/> class.
    /// </summary>
    /// <param name="payment">The payment.</param>
    /// <returns>
    /// A new instance of the <see cref="PaymentRejected"/> class.
    /// </returns>
    public static PaymentRejected CreateEvent(
        Payment? payment = null
    )
    {
        return new PaymentRejected(
            payment ?? PaymentUtils.CreatePayment()
        );
    }
}
