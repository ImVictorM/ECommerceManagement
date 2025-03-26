using Application.Common.DTOs.Results;
using Application.Common.Security.Authorization.Requests;
using Application.Orders.DTOs.Inputs;
using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

namespace Application.Orders.Commands.PlaceOrder;

/// <summary>
/// Represents a command to place an order.
/// </summary>s
/// <param name="RequestId">The current request identifier.</param>
/// <param name="ShippingMethodId">The shipping method identifier.</param>
/// <param name="Products">The order products.</param>
/// <param name="BillingAddress">The order billing address.</param>
/// <param name="DeliveryAddress">The order delivery address.</param>
/// <param name="PaymentMethod">The order payment method.</param>
/// <param name="Installments">The payment installments.</param>
/// <param name="CouponAppliedIds">The coupon applied identifiers.</param>
[Authorize(roleName: nameof(Role.Customer))]
public record PlaceOrderCommand(
    Guid RequestId,
    string ShippingMethodId,
    IEnumerable<OrderLineItemInput> Products,
    Address BillingAddress,
    Address DeliveryAddress,
    IPaymentMethod PaymentMethod,
    IEnumerable<string>? CouponAppliedIds = null,
    int? Installments = null
) : IRequestWithAuthorization<CreatedResult>;
