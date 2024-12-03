using Domain.PaymentAggregate.Abstracts;
using SharedKernel.Models;

namespace Domain.PaymentAggregate.ValueObjects;

/// <summary>
/// Represents a credit card payment method.
/// </summary>
public class CreditCardPaymentMethod : ValueObject, INetworkTokenCardPaymentMethod
{
    /// <inheritdoc/>
    public string Type => "credit_card";
    /// <inheritdoc/>
    public string Cryptograms { get; }
    /// <inheritdoc/>
    public string Number { get; }
    /// <inheritdoc/>
    public string HolderName { get; }
    /// <inheritdoc/>
    public int ExpirationMonth { get; }
    /// <inheritdoc/>
    public int ExpirationYear { get; }

    private CreditCardPaymentMethod(
        string number,
        string holderName,
        int expirationMonth,
        int expirationYear,
        string cryptograms
    )
    {
        Number = number;
        HolderName = holderName;
        ExpirationMonth = expirationMonth;
        ExpirationYear = expirationYear;
        Cryptograms = cryptograms;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CreditCardPaymentMethod"/> class.
    /// </summary>
    /// <param name="number">The card number.</param>
    /// <param name="holderName">The card holder name.</param>
    /// <param name="expirationMonth">The card expiration month.</param>
    /// <param name="expirationYear">The card expiration year.</param>
    /// <param name="cryptograms">The card network token cryptograms.</param>
    /// <returns>A new instance of the <see cref="CreditCardPaymentMethod"/> class.</returns>
    public static CreditCardPaymentMethod Create(
        string number,
        string holderName,
        int expirationMonth,
        int expirationYear,
        string cryptograms
    )
    {
        return new CreditCardPaymentMethod(
            number,
            holderName,
            expirationMonth,
            expirationYear,
            cryptograms
        );
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Type;
        yield return Number;
        yield return HolderName;
        yield return ExpirationMonth;
        yield return ExpirationYear;
        yield return Cryptograms;
    }
}
