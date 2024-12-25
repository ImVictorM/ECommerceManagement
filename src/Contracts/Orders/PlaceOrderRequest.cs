using Contracts.Common;
using Contracts.Orders.Common;
using Contracts.Payments.Common;

namespace Contracts.Orders;

/// <summary>
/// Represents a request to place an order.
/// </summary>
/// <param name="Products">The order products.</param>
/// <param name="BillingAddress">The order billing address.</param>
/// <param name="DeliveryAddress">The order delivery address.</param>
/// <param name="PaymentMethod">The order payment method.</param>
/// <param name="installments">The installments.</param>
public record PlaceOrderRequest(
    IEnumerable<OrderProduct> Products,
    AddressContract BillingAddress,
    AddressContract DeliveryAddress,
    PaymentMethod PaymentMethod,
    int? installments = null
);
