using Domain.PaymentAggregate;
using Domain.PaymentAggregate.Events;
using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.TestUtils.Events.Payments;

/// <summary>
/// Utilities for the <see cref="PaymentAuthorized"/> class.
/// </summary>
public static class PaymentAuthorizedUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="PaymentAuthorized"/> class.
    /// </summary>
    /// <param name="payment">The payment.</param>
    /// <returns>
    /// A new instance of the <see cref="PaymentAuthorized"/> class.
    /// </returns>
    public static PaymentAuthorized CreateEvent(
        Payment? payment = null
    )
    {
        return new PaymentAuthorized(
            payment ?? PaymentUtils.CreatePayment()
        );
    }
}
