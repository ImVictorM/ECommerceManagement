using SharedKernel.Models;

namespace Domain.PaymentAggregate.ValueObjects;

/// <summary>
/// Holds the status change history of a payment.
/// </summary>
public sealed class PaymentStatusHistory : ValueObject
{
    /// <summary>
    /// Gets the payment status id.
    /// </summary>
    public long PaymentStatusId { get; }

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
    /// <param name="paymentStatusId">The payment status.</param>
    private PaymentStatusHistory(long paymentStatusId)
    {
        PaymentStatusId = paymentStatusId;

        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PaymentStatusHistory"/> class.
    /// </summary>
    /// <param name="paymentStatusId">The payment status id.</param>
    /// <returns>A new instance of the <see cref="PaymentStatusHistory"/> class.</returns>
    public static PaymentStatusHistory Create(long paymentStatusId)
    {
        return new PaymentStatusHistory(paymentStatusId);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return PaymentStatusId;
        yield return CreatedAt;
    }
}
