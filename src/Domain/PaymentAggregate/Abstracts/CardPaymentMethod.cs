using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace Domain.PaymentAggregate.Abstracts;

/// <summary>
/// Represents a card payment method.
/// </summary>
public abstract class CardPaymentMethod : ValueObject, IPaymentMethod
{
    /// <summary>
    /// Gets the card type.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the tokenized card details.
    /// </summary>
    public string Token { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="CardPaymentMethod"/> method.
    /// </summary>
    /// <param name="type">The payment type.</param>
    /// <param name="token">The tokenized card details.</param>
    protected CardPaymentMethod(
        string type,
        string token
    )
    {
        Type = type;
        Token = token;
    }
}
