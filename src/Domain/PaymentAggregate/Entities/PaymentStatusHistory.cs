using Domain.Common.Models;
using Domain.PaymentAggregate.ValueObjects;

namespace Domain.PaymentAggregate.Entities;

/// <summary>
/// Holds the status change history of a payment.
/// </summary>
public sealed class PaymentStatusHistory : Entity<PaymentStatusHistoryId>
{
    /// <summary>
    /// Gets the payment statuses.
    /// </summary>
    public IEnumerable<PaymentStatusId> PaymentStatuses { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentStatusHistory"/> class.
    /// </summary>
    private PaymentStatusHistory() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentStatusHistory"/> class.
    /// </summary>
    /// <param name="paymentStatuses">The payment statuses.</param>
    private PaymentStatusHistory(IEnumerable<PaymentStatusId> paymentStatuses)
        : base(PaymentStatusHistoryId.Create())
    {
        PaymentStatuses = paymentStatuses;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PaymentStatusHistory"/> class.
    /// </summary>
    /// <param name="paymentStatuses">The payment statuses.</param>
    /// <returns>A new instance of the <see cref="PaymentStatusHistory"/> class.</returns>
    public static PaymentStatusHistory Create(IEnumerable<PaymentStatusId> paymentStatuses)
    {
        return new PaymentStatusHistory(paymentStatuses);
    }
}
