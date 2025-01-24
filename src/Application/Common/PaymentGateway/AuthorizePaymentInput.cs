using Domain.OrderAggregate;
using Domain.UserAggregate;

using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

namespace Application.Common.PaymentGateway;

/// <summary>
/// Represents an input to authorize a payment.
/// </summary>
/// <param name="requestId">The unique request identifier.</param>
/// <param name="order">The order.</param>
/// <param name="paymentMethod">The payment method.</param>
/// <param name="payer">Who is going to be charged.</param>
/// <param name="billingAddress">The billing address.</param>
/// <param name="installments">The payment installments.</param>
public record AuthorizePaymentInput
(
    Guid requestId,
    Order order,
    IPaymentMethod paymentMethod,
    User? payer = null,
    Address? billingAddress = null,
    int? installments = null
);
