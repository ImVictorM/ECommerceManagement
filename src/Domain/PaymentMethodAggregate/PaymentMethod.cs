using Domain.Common.Models;
using Domain.PaymentMethodAggregate.ValueObjects;

namespace Domain.PaymentMethodAggregate;

/// <summary>
/// Represents the payment method.
/// </summary>
public sealed class PaymentMethod : AggregateRoot<PaymentMethodId>
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
