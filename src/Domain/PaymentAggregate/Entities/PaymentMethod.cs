using Domain.Common.Models;
using Domain.PaymentAggregate.ValueObjects;

namespace Domain.PaymentAggregate.Entities;

/// <summary>
/// Represents the payment method of a payment.
/// </summary>
public sealed class PaymentMethod : Entity<PaymentMethodId>
{
    /// <summary>
    /// Gets the payment method.
    /// </summary>
    public string Method { get; private set; } = string.Empty;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentMethod"/> class.
    /// </summary>
    private PaymentMethod() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentMethod"/> class.
    /// </summary>
    /// <param name="method">The payment method.</param>
    private PaymentMethod(string method) : base(PaymentMethodId.Create())
    {
        Method = method;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PaymentMethod"/> class.
    /// </summary>
    /// <param name="method">The payment method.</param>
    /// <returns>A new instance of the <see cref="PaymentMethod"/> class.</returns>
    public static PaymentMethod Create(string method)
    {
        return new PaymentMethod(method);
    }
}
