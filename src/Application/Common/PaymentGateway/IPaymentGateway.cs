using Domain.PaymentAggregate.ValueObjects;

namespace Application.Common.PaymentGateway;

/// <summary>
/// Interface that defines methods related to payment management.
/// </summary>
public interface IPaymentGateway
{
    /// <summary>
    /// Pre-authorizes a payment, reserving the amount to be deducted.
    /// </summary>
    /// <param name="request">The authorize payment request.</param>
    /// <returns>The payment details.</returns>
    Task<PaymentResponse> AuthorizePaymentAsync(AuthorizePaymentRequest request);

    /// <summary>
    /// Completes the payment after it has been pre-authorized,
    /// deducting the reserved amount from the client.
    /// </summary>
    /// <param name="paymentId">The payment identifier.</param>
    /// <returns>A payment status response.</returns>
    Task<PaymentStatusResponse> CapturePaymentAsync(PaymentId paymentId);

    /// <summary>
    /// Cancels a pre-authorized payment. The reserved amount is
    /// released back to the customer without charging them.
    /// </summary>
    /// <param name="paymentId">The payment identifier.</param>
    /// <returns>A payment status response.</returns>
    Task<PaymentStatusResponse> CancelAuthorizationAsync(PaymentId paymentId);

    /// <summary>
    /// Refunds total amount to the customer after the payment has been captured.
    /// </summary>
    /// <param name="paymentId">The payment identifier.</param>
    /// <param name="amount">The amount to be refunded.</param>
    /// <returns>A payment refund response.</returns>
    Task<PaymentRefundResponse> RefundPaymentAsync(PaymentId paymentId, decimal amount);

    /// <summary>
    /// Retrieves a payment by its identifier.
    /// </summary>
    /// <param name="paymentId">The payment identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The payment details.</returns>
    Task<PaymentResponse> GetPaymentByIdAsync(
        PaymentId paymentId,
        CancellationToken cancellationToken = default
    );
}
