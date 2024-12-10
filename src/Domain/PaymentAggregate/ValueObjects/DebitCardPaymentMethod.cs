using Domain.PaymentAggregate.Abstracts;

namespace Domain.PaymentAggregate.ValueObjects;

/// <summary>
/// Represents a debit card payment method.
/// </summary>
public class DebitCardPaymentMethod : CardPaymentMethod
{
    private DebitCardPaymentMethod(string token)
        : base("debit_card", token)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="DebitCardPaymentMethod"/> class.
    /// </summary>
    /// <param name="token">The tokenized card data.</param>
    /// <returns>A new instance of the <see cref="DebitCardPaymentMethod"/> class.</returns>
    public static DebitCardPaymentMethod Create(string token)
    {
        return new DebitCardPaymentMethod(token);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Type;
        yield return Token;
    }
}
