using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate.Enumerations;
using Domain.PaymentAggregate.Events;
using Domain.PaymentAggregate.ValueObjects;

using SharedKernel.Models;

namespace Domain.PaymentAggregate;

/// <summary>
/// Represents a payment.
/// </summary>
public class Payment : AggregateRoot<PaymentId>
{
    /// <summary>
    /// The payment order id.
    /// </summary>
    public OrderId OrderId { get; } = null!;

    /// <summary>
    /// The current payment status id.
    /// </summary>
    public long PaymentStatusId { get; private set; }

    private Payment() { }

    private Payment(PaymentId id, OrderId orderId, PaymentStatus paymentStatus) : base(id)
    {
        OrderId = orderId;

        UpdatePaymentStatus(paymentStatus);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Payment"/> class.
    /// </summary>
    /// <param name="id">The payment id.</param>
    /// <param name="orderId">The order id.</param>
    /// <param name="paymentStatus">The initial payment status.</param>
    /// <returns>A new instance of the <see cref="Payment"/> class.</returns>
    public static Payment Create(PaymentId id, OrderId orderId, PaymentStatus paymentStatus)
    {
        return new Payment(id, orderId, paymentStatus);
    }

    /// <summary>
    /// Updates the payment status.
    /// </summary>
    /// <param name="status">The new status.</param>
    public void UpdatePaymentStatus(PaymentStatus status)
    {
        PaymentStatusId = status.Id;

        if (status == PaymentStatus.Authorized)
        {
            AddDomainEvent(new PaymentAuthorized(this));
        }
        else if (status == PaymentStatus.Approved)
        {
            AddDomainEvent(new PaymentApproved(this));
        }
        else if (status == PaymentStatus.Rejected)
        {
            AddDomainEvent(new PaymentRejected(this));
        }
        else if (status == PaymentStatus.Canceled)
        {
            AddDomainEvent(new PaymentCanceled(this));
        }
    }
}
