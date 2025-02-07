using Domain.PaymentAggregate.ValueObjects;

namespace Application.Orders.DTOs;

/// <summary>
/// Represents an order payment.
/// </summary>
/// <param name="PaymentId">The payment id.</param>
/// <param name="Amount">The payment amount.</param>
/// <param name="Installments">The quantity of installments.</param>
/// <param name="Status">The payment status.</param>
/// <param name="Details">The payment details.</param>
/// <param name="PaymentMethod">The payment method.</param>
public record OrderPaymentResult(
    PaymentId PaymentId,
    decimal Amount,
    int Installments,
    string Status,
    string Details,
    string PaymentMethod
);
