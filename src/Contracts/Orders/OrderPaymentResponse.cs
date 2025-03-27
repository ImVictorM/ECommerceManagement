namespace Contracts.Orders;

/// <summary>
/// Represents an order payment response.
/// </summary>
/// <param name="PaymentId">The payment identifier.</param>
/// <param name="Amount">The payment amount.</param>
/// <param name="Installments">The payment installments.</param>
/// <param name="Status">The payment status.</param>
/// <param name="Details">The payment details.</param>
/// <param name="PaymentMethod">The payment method.</param>
public record OrderPaymentResponse(
    string PaymentId,
    decimal Amount,
    int Installments,
    string Status,
    string Details,
    string PaymentMethod
);
