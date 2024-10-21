using Domain.Common.Errors;
using Domain.Common.Models;
using Domain.PaymentAggregate.ValueObjects;

namespace Domain.PaymentAggregate.Entities;

/// <summary>
/// Represents a payment method.
/// </summary>
public sealed class PaymentMethod : Entity<PaymentMethodId>
{
    /// <summary>
    /// Represents a credit card payment method.
    /// </summary>
    public static readonly PaymentMethod CreditCard = new(PaymentMethodId.Create(1), "credit_card");
    /// <summary>
    /// Represents a PIX payment method.
    /// </summary>
    public static readonly PaymentMethod Pix = new(PaymentMethodId.Create(2), nameof(Pix).ToLowerInvariant());
    /// <summary>
    /// Represents a bank transfer payment method.
    /// </summary>
    public static readonly PaymentMethod BankTransfer = new(PaymentMethodId.Create(3), "bank_transfer");
    /// <summary>
    /// Represents a cash payment method.
    /// </summary>
    public static readonly PaymentMethod Cash = new(PaymentMethodId.Create(4), nameof(Cash).ToLowerInvariant());

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
    /// <param name="id">The payment method identifier.</param>
    /// <param name="name">The payment method name.</param>
    private PaymentMethod(PaymentMethodId id, string name) : base(id)
    {
        Name = name;

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PaymentMethod"/> class.
    /// </summary>
    /// <param name="name">The payment method name.</param>
    /// <returns>A new instance of the <see cref="PaymentMethod"/> class.</returns>
    public static PaymentMethod Create(string name)
    {
        return GetPaymentMethodByName(name) ?? throw new DomainValidationException($"The {name} payment method does not exist");
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
}
