using Domain.OrderAggregate.Interfaces;
using Domain.OrderAggregate.ValueObjects;

namespace Domain.OrderAggregate.Services;

/// <summary>
/// Represents order product services.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Prepares the products for the order by reserving the necessary
    /// quantity of items in the inventory for each product
    /// </summary>
    /// <param name="orderProducts">The order products input.</param>
    /// <returns>A list of prepared order products.</returns>
    IAsyncEnumerable<OrderProduct> PrepareOrderProductsAsync(IEnumerable<IOrderProductReserved> orderProducts);

    /// <summary>
    /// Calculates the total amount of the products, applying the necessary discounts.
    /// </summary>
    /// <param name="orderProducts">The products to calculate the total.</param>
    /// <param name="couponsApplied">The coupons applied to the order.</param>
    /// <returns>The total amount.</returns>
    Task<decimal> CalculateTotalAsync(IEnumerable<OrderProduct> orderProducts, IEnumerable<OrderCoupon>? couponsApplied);
}
