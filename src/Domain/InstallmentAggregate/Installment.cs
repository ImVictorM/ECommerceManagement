using Domain.Common.Models;
using Domain.InstallmentAggregate.ValueObjects;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate.ValueObjects;

namespace Domain.InstallmentAggregate;

/// <summary>
/// Represents an installment.
/// </summary>
public sealed class Installment : Entity<InstallmentId>
{
    /// <summary>
    /// Gets the installment quantity of payments required.
    /// </summary>
    public int QuantityPayments { get; private set; }
    /// <summary>
    /// Gets the amount to be paid per payment.
    /// </summary>
    public float AmountPerPayment { get; private set; }
    /// <summary>
    /// Gets the installment order id.
    /// </summary>
    public OrderId OrderId { get; private set; } = null!;
    /// <summary>
    /// Gets the related payment ids.
    /// </summary>
    public IEnumerable<PaymentId>? PaymentIds { get; private set; }

    /// <summary>
    /// Initiates a new instance of the <see cref="Installment"/> class.
    /// </summary>
    private Installment() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="Installment"/> class.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <param name="quantityPayments">The quantity of payments required.</param>
    /// <param name="amountPerPayment">The amount per payment required.</param>
    /// <param name="paymentIds">The payments related list.</param>
    private Installment(
        OrderId orderId,
        int quantityPayments,
        float amountPerPayment,
        IEnumerable<PaymentId>? paymentIds
    ) : base(InstallmentId.Create())
    {
        QuantityPayments = quantityPayments;
        AmountPerPayment = amountPerPayment;
        OrderId = orderId;
        PaymentIds = paymentIds;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Installment"/> class.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <param name="quantityPayments">The quantity of payments required.</param>
    /// <param name="amountPerPayment">The amount per payment required.</param>
    /// <param name="paymentIds">The payments related list.</param>
    /// <returns>A new instance of the <see cref="Installment"/> class.</returns>
    public static Installment Create(
        OrderId orderId,
        int quantityPayments,
        float amountPerPayment,
        IEnumerable<PaymentId>? paymentIds
    )
    {
        return new Installment(
            orderId,
            quantityPayments,
            amountPerPayment,
            paymentIds
        );
    }
}
