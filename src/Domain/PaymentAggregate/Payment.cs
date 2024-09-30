using Domain.Common.Models;
using Domain.InstallmentAggregate.ValueObjects;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate.Entities;
using Domain.PaymentAggregate.ValueObjects;
using Domain.PaymentMethodAggregate.ValueObjects;
using Domain.PaymentStatusAggregate.ValueObjects;

namespace Domain.PaymentAggregate;

/// <summary>
/// Represents a payment
/// </summary>
public sealed class Payment : AggregateRoot<PaymentId>
{
    /// <summary>
    /// The payment status change history.
    /// </summary>
    private readonly List<PaymentStatusHistory> _paymentStatusHistory = [];

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
    public PaymentMethodId PaymentMethodId { get; private set; } = null!;
    /// <summary>
    /// Gets the payment status.
    /// </summary>
    public PaymentStatusId PaymentStatusId { get; private set; } = null!;
    /// <summary>
    /// Gets the payment status change history.
    /// </summary>
    public IReadOnlyList<PaymentStatusHistory> PaymentStatusHistories => _paymentStatusHistory.AsReadOnly();

    /// <summary>
    /// Initiates a new instance of the <see cref="Payment"/> class.
    /// </summary>
    private Payment() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="Payment"/> class.
    /// </summary>
    /// <param name="amount">The payment amount.</param>
    /// <param name="orderId">The order id.</param>
    /// <param name="paymentMethodId">The payment method.</param>
    /// <param name="paymentStatusId">The status of the payment.</param>
    /// <param name="installmentId">The installment id (optional).</param>
    private Payment(
        float amount,
        OrderId orderId,
        PaymentMethodId paymentMethodId,
        PaymentStatusId paymentStatusId,
        InstallmentId? installmentId = null
    )
    {
        Amount = amount;
        OrderId = orderId;
        PaymentMethodId = paymentMethodId;
        PaymentStatusId = paymentStatusId;
        InstallmentId = installmentId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Payment"/> class.
    /// </summary>
    /// <param name="amount">The payment amount.</param>
    /// <param name="orderId">The order id.</param>
    /// <param name="paymentMethodId">The payment method.</param>
    /// <param name="paymentStatusId">The status of the payment.</param>
    /// <param name="installmentId">The installment id (optional).</param>
    /// <returns>A new instance of the <see cref="Payment"/> class.</returns>
    public static Payment Create(
        float amount,
        OrderId orderId,
        PaymentMethodId paymentMethodId,
        PaymentStatusId paymentStatusId,
        InstallmentId? installmentId = null
    )
    {
        return new Payment(
            amount,
            orderId,
            paymentMethodId,
            paymentStatusId,
            installmentId
        );
    }
}
