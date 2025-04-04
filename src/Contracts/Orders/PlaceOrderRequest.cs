using Contracts.Common;
using Contracts.Common.PaymentMethods;

namespace Contracts.Orders;

/// <summary>
/// Represents a request to place an order.
/// </summary>
/// <param name="ShippingMethodId">The shipping method identifier.</param>
/// <param name="Products">The order products.</param>
/// <param name="BillingAddress">The order billing address.</param>
/// <param name="DeliveryAddress">The order delivery address.</param>
/// <param name="PaymentMethod">The order payment method.</param>
/// <param name="installments">The order installments.</param>
/// <param name="CouponAppliedIds">The coupon applied identifiers.</param>
public record PlaceOrderRequest(
    string ShippingMethodId,
    IEnumerable<OrderLineItemRequest> Products,
    AddressContract BillingAddress,
    AddressContract DeliveryAddress,
    BasePaymentMethod PaymentMethod,
    IEnumerable<string>? CouponAppliedIds = null,
    int? installments = null
);
