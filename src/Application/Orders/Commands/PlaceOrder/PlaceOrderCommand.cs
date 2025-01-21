using Application.Common.DTOs;
using Application.Orders.Commands.Common.DTOs;

using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Authorization;
using Application.Common.Security.Authorization.Roles;

namespace Application.Orders.Commands.PlaceOrder;

/// <summary>
/// Represents a command to place an order.
/// </summary>s
/// <param name="requestId">The current request identifier.</param>
/// <param name="Products">The order products.</param>
/// <param name="BillingAddress">The order billing address.</param>
/// <param name="DeliveryAddress">The order delivery address.</param>
/// <param name="PaymentMethod">The order payment method.</param>
/// <param name="Installments">The payment installments.</param>
/// <param name="CouponAppliedIds">The coupon ids applied.</param>
[Authorize(roleName: nameof(Role.Customer))]
public record PlaceOrderCommand(
    Guid requestId,
    IEnumerable<OrderProductInput> Products,
    Address BillingAddress,
    Address DeliveryAddress,
    IPaymentMethod PaymentMethod,
    IEnumerable<string>? CouponAppliedIds = null,
    int? Installments = null
) : IRequestWithAuthorization<CreatedResult>
{
    /// <inheritdoc/>
    public string? UserId => null;
}
