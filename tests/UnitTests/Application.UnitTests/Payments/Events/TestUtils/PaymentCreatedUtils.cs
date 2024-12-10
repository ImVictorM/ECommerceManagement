using Domain.PaymentAggregate;
using Domain.PaymentAggregate.Events;
using Domain.UnitTests.TestUtils;
using Domain.UnitTests.TestUtils.Constants;
using Domain.UserAggregate.ValueObjects;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

namespace Application.UnitTests.Payments.Events.TestUtils;

/// <summary>
/// Utilities for the <see cref="PaymentCreated"/> event.
/// </summary>
public static class PaymentCreatedUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="PaymentCreated"/> class.
    /// </summary>
    /// <param name="payment">The payment created.</param>
    /// <param name="payerId">The payment payer id.</param>
    /// <param name="billingAddress">The billing address.</param>
    /// <param name="deliveryAddress">The delivery address.</param>
    /// <returns> Anew instance of the <see cref="PaymentCreated"/> class.</returns>
    public static PaymentCreated CreateEvent(
        Payment? payment = null,
        UserId? payerId = null,
        Address? billingAddress = null,
        Address? deliveryAddress = null
    )
    {
        return new PaymentCreated(
            payment ?? PaymentUtils.CreatePayment(),
            payerId ?? DomainConstants.User.Id,
            billingAddress ?? AddressUtils.CreateAddress(),
            deliveryAddress ?? AddressUtils.CreateAddress()
        );
    }
}
