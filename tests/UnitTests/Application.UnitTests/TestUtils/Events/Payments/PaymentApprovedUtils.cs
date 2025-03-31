using Domain.PaymentAggregate;
using Domain.PaymentAggregate.Events;
using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.TestUtils.Events.Payments;

/// <summary>
/// Utilities for the <see cref="PaymentApproved"/> class.
/// </summary>
public static class PaymentApprovedUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="PaymentApproved"/> class.
    /// </summary>
    /// <param name="payment">The payment.</param>
    /// <returns>
    /// A new instance of the <see cref="PaymentApproved"/> class.
    /// </returns>
    public static PaymentApproved CreateEvent(
        Payment? payment = null
    )
    {
        return new PaymentApproved(
            payment ?? PaymentUtils.CreatePayment()
        );
    }
}
