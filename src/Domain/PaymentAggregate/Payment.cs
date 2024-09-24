using Domain.Common.Models;
using Domain.InstallmentAggregate.ValueObjects;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate.Entities;
using Domain.PaymentAggregate.ValueObjects;

namespace Domain.PaymentAggregate;

/// <summary>
/// Represents a payment
/// </summary>
public sealed class Payment : AggregateRoot<PaymentId>
{
    /// <summary>
    /// Gets the payment amount.
    /// </summary>
    public float Amount { get; private set; }
    /// <summary>
    /// Gets the installment this payment references. If it is null, the payment doesn't refer to any installment.
    /// </summary>
    public InstallmentId? InstallmentId { get; private set; }
    /// <summary>
    /// Gets the order id of this payment.
    /// </summary>
    public OrderId OrderId { get; private set; } = null!;
    /// <summary>
    /// Gets the payment method.
    /// </summary>
    public PaymentMethod PaymentMethod { get; private set; } = null!;
    /// <summary>
    /// Gets the payment status.
    /// </summary>
    public PaymentStatus PaymentStatus { get; private set; } = null!;
    /// <summary>
    /// Gets the payment status change history.
    /// </summary>
    public PaymentStatusHistory PaymentStatusHistory { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="Payment"/> class.
    /// </summary>
    private Payment() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="Payment"/> class.
    /// </summary>
    /// <param name="amount">The payment amount.</param>
    /// <param name="orderId">The order id.</param>
    /// <param name="paymentMethod">The payment method.</param>
    /// <param name="paymentStatusHistory">The payment status change history.</param>
    /// <param name="installmentId">The installment id (optional).</param>
    private Payment(
        float amount,
        OrderId orderId,
        PaymentMethod paymentMethod,
        PaymentStatus paymentStatus,
        PaymentStatusHistory paymentStatusHistory,
        InstallmentId? installmentId = null
    ) : base(PaymentId.Create())
    {
        Amount = amount;
        OrderId = orderId;
        PaymentMethod = paymentMethod;
        PaymentStatus = paymentStatus;
        PaymentStatusHistory = paymentStatusHistory;
        InstallmentId = installmentId;
    }

    public static Payment Create(
        float amount,
        OrderId orderId,
        PaymentMethod paymentMethod,
        PaymentStatus paymentStatus,
        PaymentStatusHistory paymentStatusHistory,
        InstallmentId? installmentId = null
    )
    {
        return new Payment(
            amount,
            orderId,
            paymentMethod,
            paymentStatus,
            paymentStatusHistory,
            installmentId
        );
    }
}
