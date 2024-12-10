using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate.Enumeration;
using Domain.PaymentAggregate.Events;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using SharedKernel.Errors;
using SharedKernel.Interfaces;
using SharedKernel.Models;
using SharedKernel.ValueObjects;

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
    /// Gest the payment description.
    /// </summary>
    public string Description { get; private set; } = null!;
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
        Description = "Payment in progress. Waiting for authorization";
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Payment"/> class.
    /// </summary>
    /// <param name="amount">The payment amount.</param>
    /// <param name="orderId">The order id.</param>
    /// <param name="payerId">The payer id.</param>
    /// <param name="paymentMethod">The payment method.</param>
    /// <param name="billingAddress">The billing address.</param>
    /// <param name="deliveryAddress">The delivery address.</param>
    /// <param name="installments">The quantity of installments (optional).</param>
    /// <returns>A new instance of the <see cref="Payment"/> class.</returns>
    public static Payment Create(
        decimal amount,
        OrderId orderId,
        UserId payerId,
        IPaymentMethod paymentMethod,
        Address billingAddress,
        Address deliveryAddress,
        int? installments = null
    )
    {
        var payment = new Payment(
            amount,
            orderId,
            paymentMethod,
            PaymentStatus.InProgress,
            installments
        );

        payment.AddDomainEvent(new PaymentCreated(payment, payerId, billingAddress, deliveryAddress));

        return payment;
    }

    /// <summary>
    /// Cancels a payment for a given reason.
    /// </summary>
    /// <param name="reason">The reason the payment was canceled.</param>
    public void CancelPayment(string reason)
    {
        if (
            PaymentStatusId == PaymentStatus.Pending.Id
            || PaymentStatusId == PaymentStatus.InProgress.Id
        )
        {
            UpdatePaymentStatus(PaymentStatus.Canceled, reason);
            AddDomainEvent(new PaymentCanceled(this));
            return;
        }

        throw new BaseException(
            "The current payment cannot be canceled. Only payments pending or in progress can be canceled",
            "Cancel Payment Error",
            ErrorCode.ValidationError
        );
    }

    private void UpdatePaymentStatus(PaymentStatus status, string description)
    {
        PaymentStatusId = status.Id;
        _paymentStatusHistories.Add(PaymentStatusHistory.Create(PaymentStatusId));
        Description = description;
    }
}
