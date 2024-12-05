using Application.Common.Interfaces.Payments;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UserAggregate;
using SharedKernel.ValueObjects;

namespace Application.Common.Interfaces.Services;

/// <summary>
/// Interface that defines methods related to payment management.
/// </summary>
public interface IPaymentGateway
{
    /// <summary>
    /// Pre-authorizes a payment, reserving the amount to be deducted.
    /// </summary>
    /// <param name="payment">The payment to be pre authorized.</param>
    /// <param name="payer">Who is going to be charged.</param>
    /// <param name="billingAddress">The billing address.</param>
    /// <param name="deliveryAddress">The order delivery address (optional).</param>
    /// <returns>An authorize payment response.</returns>
    Task<IAuthorizePaymentResponse> AuthorizePaymentAsync(
        Payment payment,
        User? payer = null,
        Address? billingAddress = null,
        Address? deliveryAddress = null
    );
    /// <summary>
    /// Completes the payment after it has been pre-authorized, deducting the reserved amount from the client.
    /// </summary>
    /// <param name="paymentId">The payment identifier.</param>
    /// <returns>A payment status response.</returns>
    Task<IPaymentStatusResponse> CapturePaymentAsync(PaymentId paymentId);
    /// <summary>
    /// Cancels a pre-authorized payment. The reserved amount is released back to the customer without charging them.
    /// </summary>
    /// <param name="paymentId">The payment identifier.</param>
    /// <returns>A payment status response.</returns>
    Task<IPaymentStatusResponse> CancelAuthorizationAsync(PaymentId paymentId);
    /// <summary>
    /// Refunds total amount to the customer after the payment has been captured.
    /// </summary>
    /// <param name="paymentId">The payment identifier.</param>
    /// <param name="amount">The amount to be refunded.</param>
    /// <returns>A payment refund response.</returns>
    Task<IPaymentRefundResponse> RefundPaymentAsync(PaymentId paymentId, decimal amount);
}
