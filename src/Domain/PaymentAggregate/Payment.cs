using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate.Enumeration;
using Domain.PaymentAggregate.ValueObjects;
using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace Domain.PaymentAggregate;

/// <summary>
/// Represents a payment
/// </summary>
public sealed class Payment : AggregateRoot<PaymentId>
{
    private readonly List<PaymentStatusHistory> _paymentStatusHistories = [];

    /// <summary>
    /// Gets the payment amount.
    /// </summary>
    public decimal Amount { get; private set; }
    /// <summary>
    /// Gets the installment for this payment references.
    /// </summary>
    public int Installments { get; private set; } = 1;
    /// <summary>
    /// Gets the order id of this payment.
    /// </summary>
    public OrderId OrderId { get; private set; } = null!;
    /// <summary>
    /// Gets the payment method.
    /// </summary>
    public IPaymentMethod PaymentMethod { get; private set; } = null!;
    /// <summary>
    /// Gets the payment status.
    /// </summary>
    public long PaymentStatusId { get; private set; }
    /// <summary>
    /// Gets the payment status histories.
    /// </summary>
    public IReadOnlyList<PaymentStatusHistory> PaymentStatusHistories => _paymentStatusHistories.AsReadOnly();

    private Payment() { }

    private Payment(
        decimal amount,
        OrderId orderId,
        IPaymentMethod paymentMethod,
        PaymentStatus paymentStatus,
        int? installments = null
    )
    {
        Amount = amount;
        OrderId = orderId;
        PaymentMethod = paymentMethod;
        PaymentStatusId = paymentStatus.Id;
        Installments = installments ?? 1;
        _paymentStatusHistories.Add(PaymentStatusHistory.Create(paymentStatus.Id));
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Payment"/> class.
    /// </summary>
    /// <param name="amount">The payment amount.</param>
    /// <param name="orderId">The order id.</param>
    /// <param name="paymentMethod">The payment method.</param>
    /// <param name="installments">The quantity of installments (optional).</param>
    /// <returns>A new instance of the <see cref="Payment"/> class.</returns>
    public static Payment Create(
        decimal amount,
        OrderId orderId,
        IPaymentMethod paymentMethod,
        int? installments = null
    )
    {
        return new Payment(
            amount,
            orderId,
            paymentMethod,
            PaymentStatus.InProgress,
            installments
        );
    }
}
