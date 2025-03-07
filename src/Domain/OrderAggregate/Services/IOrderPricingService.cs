using Domain.OrderAggregate.ValueObjects;
using Domain.ShippingMethodAggregate.ValueObjects;

namespace Domain.OrderAggregate.Services;

/// <summary>
/// Defines the contract for a service related to order pricing.
/// </summary>
public interface IOrderPricingService
{
    /// <summary>
    /// Calculates the final total amount for an order.
    /// </summary>
    /// <param name="lineItems">
    /// The collection of order line items.
    /// </param>
    /// <param name="shippingMethodId">
    /// The identifier of the shipping method selected for the order.
    /// </param>
    /// <param name="couponsApplied">
    /// A collection of coupons applied to the order that may affect pricing.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// The final total order price.
    /// </returns>
    Task<decimal> CalculateTotalAsync(
        IEnumerable<OrderLineItem> lineItems,
        ShippingMethodId shippingMethodId,
        IEnumerable<OrderCoupon>? couponsApplied = default,
        CancellationToken cancellationToken = default
    );
}
