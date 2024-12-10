using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.Enumeration;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UnitTests.TestUtils.Constants;
using Domain.UserAggregate.ValueObjects;
using SharedKernel.Interfaces;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for payment.
/// </summary>
public static class PaymentUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="Payment"/> class.
    /// </summary>
    /// <param name="amount">The payment amount.</param>
    /// <param name="orderId">The payment order id.</param>
    /// <param name="payerId">The payment payer id.</param>
    /// <param name="paymentMethod">The payment method.</param>
    /// <param name="billingAddress">The payment billing address.</param>
    /// <param name="deliveryAddress">The delivery address.</param>
    /// <param name="installments">The installments quantity.</param>
    /// <param name="initialPaymentStatus">Defines a initial status for the payment.</param>
    /// <returns>A new instance of the <see cref="Payment"/> class.</returns>
    public static Payment CreatePayment(
        decimal? amount = null,
        OrderId? orderId = null,
        UserId? payerId = null,
        IPaymentMethod? paymentMethod = null,
        Address? billingAddress = null,
        Address? deliveryAddress = null,
        int? installments = null,
        PaymentStatus? initialPaymentStatus = null
    )
    {
        var payment = Payment.Create(
            amount ?? DomainConstants.Payment.Amount,
            orderId ?? DomainConstants.Payment.OrderId,
            payerId ?? DomainConstants.Payment.PayerId,
            paymentMethod ?? CreateCreditCardPayment(),
            billingAddress ?? AddressUtils.CreateAddress(),
            deliveryAddress ?? AddressUtils.CreateAddress(),
            installments
        );

        if (initialPaymentStatus is not null)
        {
            var statusProperty = typeof(Payment).GetProperty(nameof(Payment.PaymentStatusId), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (statusProperty != null && statusProperty.CanWrite)
            {
                statusProperty.SetValue(payment, initialPaymentStatus.Id);
            }
        }

        return payment;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CreditCardPaymentMethod"/> class.
    /// </summary>
    /// <returns>A new instance of the <see cref="CreditCardPaymentMethod"/> class.</returns>
    public static CreditCardPaymentMethod CreateCreditCardPayment(string? token = null)
    {
        return CreditCardPaymentMethod.Create(token ?? DomainConstants.Payment.CardToken);
    }
}
