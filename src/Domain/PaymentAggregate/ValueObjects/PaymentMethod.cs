using Domain.Common.Errors;
using Domain.Common.Models;

namespace Domain.PaymentAggregate.ValueObjects;

/// <summary>
/// Represents the payment method.
/// </summary>
public sealed class PaymentMethod : ValueObject
{
    /// <summary>
    /// Represents a credit card payment method.
    /// </summary>
    public static readonly PaymentMethod CreditCard = new("credit_card");
    /// <summary>
    /// Represents a PIX payment method.
    /// </summary>
    public static readonly PaymentMethod Pix = new(nameof(Pix).ToLowerInvariant());
    /// <summary>
    /// Represents a bank transfer payment method.
    /// </summary>
    public static readonly PaymentMethod BankTransfer = new("bank_transfer");
    /// <summary>
    /// Represents a cash payment method.
    /// </summary>
    public static readonly PaymentMethod Cash = new(nameof(Cash).ToLowerInvariant());

    /// <summary>
    /// Gets the payment method name.
    /// </summary>
    public string Name { get; } = string.Empty;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentMethod"/> class.
    /// </summary>
    private PaymentMethod() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentMethod"/> class.
    /// </summary>
    /// <param name="name">The payment method name.</param>
    private PaymentMethod(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PaymentMethod"/> class.
    /// </summary>
    /// <param name="name">The payment method name.</param>
    /// <returns>A new instance of the <see cref="PaymentMethod"/> class.</returns>
    public static PaymentMethod Create(string name)
    {
        if (GetPaymentMethodByName(name) == null) throw new DomainValidationException($"The {name} payment method does not exist");

        return new PaymentMethod(name);
    }

    /// <summary>
    /// Gets a payment method by name, or null if not found.
    /// </summary>
    /// <param name="name">The payment method name.</param>
    /// <returns>The payment method or null.</returns>
    private static PaymentMethod? GetPaymentMethodByName(string name)
    {
        return List().FirstOrDefault(paymentMethod => paymentMethod.Name == name);
    }

    /// <summary>
    /// Gets all the payment methods in a list format.
    /// </summary>
    /// <returns>All the payment mtehods.</returns>
    public static IEnumerable<PaymentMethod> List()
    {
        return
        [
            CreditCard,
            Pix,
            BankTransfer,
            Cash
        ];
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
    }
}
