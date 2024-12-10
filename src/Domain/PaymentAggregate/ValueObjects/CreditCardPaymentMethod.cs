using Domain.PaymentAggregate.Abstracts;

namespace Domain.PaymentAggregate.ValueObjects;

/// <summary>
/// Represents a credit card payment method.
/// </summary>
public class CreditCardPaymentMethod : CardPaymentMethod
{
    private CreditCardPaymentMethod(string token)
        : base("credit_card", token)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CreditCardPaymentMethod"/> class.
    /// </summary>
    /// <param name="token">The tokenized card details.</param>
    /// <returns>A new instance of the <see cref="CreditCardPaymentMethod"/> class.</returns>
    public static CreditCardPaymentMethod Create(string token)
    {
        return new CreditCardPaymentMethod(token);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Type;
        yield return Token;
    }
}
