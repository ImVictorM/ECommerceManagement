using SharedKernel.Interfaces;

namespace Domain.PaymentAggregate.Abstracts;

/// <summary>
/// Represents a card payment method.
/// </summary>
public interface ICardPaymentMethod : IPaymentMethod
{
    /// <summary>
    /// Gets the card number.
    /// </summary>
    public string Number { get; }
    /// <summary>
    /// Gets the card holder name.
    /// </summary>
    public string HolderName { get; }
    /// <summary>
    /// Gets the card expiration month.
    /// </summary>
    public int ExpirationMonth { get; }
    /// <summary>
    /// Gets the card expiration year.
    /// </summary>
    public int ExpirationYear { get; }
}
