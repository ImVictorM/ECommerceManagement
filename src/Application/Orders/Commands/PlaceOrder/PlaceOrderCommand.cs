using Application.Orders.Commands.Common.DTOs;
using MediatR;
using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

namespace Application.Orders.Commands.PlaceOrder;

/// <summary>
/// Represents a command to place an order.
/// </summary>
/// <param name="UserId">The order owner id.</param>
/// <param name="Products">The order products.</param>
/// <param name="BillingAddress">The order billing address.</param>
/// <param name="DeliveryAddress">The order delivery address.</param>
/// <param name="PaymentMethod">The order payment method.</param>
/// <param name="Installments">The payment installments.</param>
public record PlaceOrderCommand(
    string UserId,
    IEnumerable<OrderProductInput> Products,
    Address BillingAddress,
    Address DeliveryAddress,
    IPaymentMethod PaymentMethod,
    int? Installments = null
) : IRequest<Unit>;
