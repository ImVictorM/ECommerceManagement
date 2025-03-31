using Domain.PaymentAggregate;
using Domain.PaymentAggregate.Events;
using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.TestUtils.Events.Payments;

/// <summary>
/// Utilities for the <see cref="PaymentCanceled"/> class.
/// </summary>
public static class PaymentCanceledUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="PaymentCanceled"/> class.
    /// </summary>
    /// <param name="payment">The payment.</param>
    /// <returns>
    /// A new instance of the <see cref="PaymentCanceled"/> class.
    /// </returns>
    public static PaymentCanceled CreateEvent(
        Payment? payment = null
    )
    {
        return new PaymentCanceled(
            payment ?? PaymentUtils.CreatePayment()
        );
    }
}
