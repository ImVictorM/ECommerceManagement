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
    /// <param name="paymentId">The payment id.</param>
    /// <param name="orderId">The order id.</param>
    /// <param name="paymentStatus">The initial payment status.</param>
    /// <returns>A new instance of the <see cref="Payment"/> class.</returns>
    public static Payment Create(PaymentId paymentId, OrderId orderId, PaymentStatus paymentStatus)
    {
        return new Payment(paymentId, orderId, paymentStatus);
    }

    /// <summary>
    /// Updates the payment status.
    /// </summary>
    /// <param name="status">The new status.</param>
    public void UpdatePaymentStatus(PaymentStatus status)
    {
        PaymentStatusId = status.Id;

        AddDomainEvent(new PaymentStatusChanged(this));
    }
}
