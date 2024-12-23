using Domain.OrderAggregate.ValueObjects;

namespace Domain.OrderAggregate.Services;

/// <summary>
/// Represents order product services.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Calculates the total amount of the products, applying the necessary discounts.
    /// </summary>
    /// <param name="orderProducts">The products to calculate the total.</param>
    /// <param name="couponsApplied">The coupons applied to the order.</param>
    /// <returns>The total amount.</returns>
    Task<decimal> CalculateTotalAsync(IEnumerable<OrderProduct> orderProducts, IEnumerable<OrderCoupon>? couponsApplied);

    /// <summary>
    /// Verifies if all the products have available inventory to make the order.
    /// </summary>
    /// <param name="orderProducts">The products to be verified.</param>
    /// <returns>A boolean value indicating if all products have available inventory.</returns>
    Task<bool> HasInventoryAvailableAsync(IEnumerable<OrderProduct> orderProducts);
}
