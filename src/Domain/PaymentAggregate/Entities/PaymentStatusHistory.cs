using Domain.Common.Models;
using Domain.PaymentAggregate.ValueObjects;
using Domain.PaymentStatusAggregate.ValueObjects;

namespace Domain.PaymentAggregate.Entities;

/// <summary>
/// Holds the status change history of a payment.
/// </summary>
public sealed class PaymentStatusHistory : Entity<PaymentStatusHistoryId>
{
    /// <summary>
    /// Gets the payment statuses.
    /// </summary>
    public PaymentStatusId PaymentStatusId { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentStatusHistory"/> class.
    /// </summary>
    private PaymentStatusHistory() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentStatusHistory"/> class.
    /// </summary>
    /// <param name="paymentStatusId">The payment statuses.</param>
    private PaymentStatusHistory(PaymentStatusId paymentStatusId)
    {
        PaymentStatusId = paymentStatusId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PaymentStatusHistory"/> class.
    /// </summary>
    /// <param name="paymentStatusId">The payment statuses.</param>
    /// <returns>A new instance of the <see cref="PaymentStatusHistory"/> class.</returns>
    public static PaymentStatusHistory Create(PaymentStatusId paymentStatusId)
    {
        return new PaymentStatusHistory(paymentStatusId);
    }
}
