namespace Contracts.Orders.Common;

/// <summary>
/// Represents an order payment.
/// </summary>
/// <param name="PaymentId">The payment id.</param>
/// <param name="Amount">The payment amount.</param>
/// <param name="Installments">The payment installments.</param>
/// <param name="Status">The payment status.</param>
/// <param name="Description">The payment description.</param>
/// <param name="PaymentType">The payment type.</param>
public record OrderPayment(
    string PaymentId,
    decimal Amount,
    int Installments,
    string Status,
    string Description,
    string PaymentType
);
