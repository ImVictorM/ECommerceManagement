using Domain.Common.Models;

namespace Domain.PaymentAggregate.ValueObjects;

/// <summary>
/// Holds the status change history of a payment.
/// </summary>
public sealed class PaymentStatusHistory : ValueObject
{
    /// <summary>
    /// Gets the payment status.
    /// </summary>
    public PaymentStatus PaymentStatus { get; } = null!;

    /// <summary>
    /// Gets the date the payment status was created;
    /// </summary>
    public DateTimeOffset CreatedAt { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentStatusHistory"/> class.
    /// </summary>
    private PaymentStatusHistory() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentStatusHistory"/> class.
    /// </summary>
    /// <param name="paymentStatus">The payment status.</param>
    private PaymentStatusHistory(PaymentStatus paymentStatus)
    {
        PaymentStatus = paymentStatus;

        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PaymentStatusHistory"/> class.
    /// </summary>
    /// <param name="paymentStatus">The payment status.</param>
    /// <returns>A new instance of the <see cref="PaymentStatusHistory"/> class.</returns>
    public static PaymentStatusHistory Create(PaymentStatus paymentStatus)
    {
        return new PaymentStatusHistory(paymentStatus);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return PaymentStatus;
        yield return CreatedAt;
    }
}
