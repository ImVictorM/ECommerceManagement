using Application.Common.DTOs;
using Application.Orders.Commands.Common.DTOs;

using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

using MediatR;

namespace Application.Orders.Commands.PlaceOrder;

/// <summary>
/// Represents a command to place an order.
/// </summary>s
/// <param name="CurrentUserId">The order owner id.</param>
/// <param name="Products">The order products.</param>
/// <param name="BillingAddress">The order billing address.</param>
/// <param name="DeliveryAddress">The order delivery address.</param>
/// <param name="PaymentMethod">The order payment method.</param>
/// <param name="Installments">The payment installments.</param>
/// <param name="CouponAppliedIds">The coupon ids applied.</param>
public record PlaceOrderCommand(
    string CurrentUserId,
    IEnumerable<OrderProductInput> Products,
    Address BillingAddress,
    Address DeliveryAddress,
    IPaymentMethod PaymentMethod,
    IEnumerable<string>? CouponAppliedIds = null,
    int? Installments = null
) : IRequest<CreatedResult>;
