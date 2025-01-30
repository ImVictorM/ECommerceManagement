using Contracts.Common;
using Contracts.Common.PaymentMethods;

namespace Contracts.Orders;

/// <summary>
/// Represents a request to place an order.
/// </summary>
/// <param name="Products">The order products.</param>
/// <param name="BillingAddress">The order billing address.</param>
/// <param name="DeliveryAddress">The order delivery address.</param>
/// <param name="PaymentMethod">The order payment method.</param>
/// <param name="installments">The order installments.</param>
/// <param name="CouponAppliedIds">The coupon applied ids.</param>
public record PlaceOrderRequest(
    IEnumerable<OrderProductRequest> Products,
    AddressContract BillingAddress,
    AddressContract DeliveryAddress,
    PaymentMethod PaymentMethod,
    IEnumerable<string>? CouponAppliedIds = null,
    int? installments = null
);
