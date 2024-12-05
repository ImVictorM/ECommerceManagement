using Domain.PaymentAggregate.ValueObjects;
using Domain.UnitTests.TestUtils.Constants;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for payment.
/// </summary>
public static class PaymentUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="CreditCardPaymentMethod"/> class.
    /// </summary>
    /// <param name="cardNumber">The credit card number.</param>
    /// <param name="cardHolder">The credit card holder name.</param>
    /// <param name="expirationMonth">The credit card expiration month.</param>
    /// <param name="expirationYear">The credit card expiration year.</param>
    /// <param name="cryptograms">The credit card cryptograms.</param>
    /// <returns>A new instance of the <see cref="CreditCardPaymentMethod"/> class.</returns>
    public static CreditCardPaymentMethod CreateCreditCardPayment(
        string? cardNumber = null,
        string? cardHolder = null,
        int? expirationMonth = null,
        int? expirationYear = null,
        string? cryptograms = null)
    {
        return CreditCardPaymentMethod.Create(
            cardNumber ?? DomainConstants.Payment.CardNumber,
            cardHolder ?? DomainConstants.Payment.CardHolder,
            expirationMonth ?? DomainConstants.Payment.CardExpirationMonth,
            expirationYear ?? DomainConstants.Payment.CardExpirationYear,
            cryptograms ?? DomainConstants.Payment.CardCryptograms
        );
    }
}
