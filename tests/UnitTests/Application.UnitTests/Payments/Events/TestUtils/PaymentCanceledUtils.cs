using Domain.PaymentAggregate;
using Domain.PaymentAggregate.Events;
using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Payments.Events.TestUtils;

/// <summary>
/// Utilities for the <see cref="PaymentCanceled"/> event.
/// </summary>
public static class PaymentCanceledUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="PaymentCanceled"/> class.
    /// </summary>
    /// <param name="payment">The payment that was canceled.</param>
    /// <returns>A new instance of the <see cref="PaymentCanceled"/> class.</returns>
    public static PaymentCanceled CreateEvent(Payment? payment = null)
    {
        return new PaymentCanceled(payment ?? PaymentUtils.CreatePayment());
    }
}
