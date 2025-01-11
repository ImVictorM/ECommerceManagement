using Domain.OrderAggregate;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UserAggregate;
using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

namespace Application.Common.Interfaces.Payments;

/// <summary>
/// Interface that defines methods related to payment management.
/// </summary>
public interface IPaymentGateway
{
    /// <summary>
    /// Pre-authorizes a payment, reserving the amount to be deducted.
    /// </summary>
    /// <param name="requestId">The unique request identifier.</param>
    /// <param name="order">The order.</param>
    /// <param name="paymentMethod">The payment method.</param>
    /// <param name="payer">Who is going to be charged.</param>
    /// <param name="billingAddress">The billing address.</param>
    /// <param name="installments">The payment installments.</param>
    /// <returns>The payment details.</returns>
    Task<IPaymentResponse> AuthorizePaymentAsync(
        Guid requestId,
        Order order,
        IPaymentMethod paymentMethod,
        User? payer = null,
        Address? billingAddress = null,
        int? installments = null
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

    /// <summary>
    /// Retrieves a payment by its identifier.
    /// </summary>
    /// <param name="paymentId">The payment id.</param>
    /// <returns>The payment details.</returns>
    Task<IPaymentResponse> GetPaymentByIdAsync(PaymentId paymentId);
}
