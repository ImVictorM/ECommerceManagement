using Domain.OrderAggregate.ValueObjects;

namespace Domain.OrderAggregate.Services;

/// <summary>
/// Represents order product services.
/// </summary>
public interface IOrderProductService
{
    /// <summary>
    /// Calculates the total amount of the products, applying the necessary discounts.
    /// </summary>
    /// <param name="orderProducts">The products to calculate the total.</param>
    /// <returns>The total amount.</returns>
    Task<decimal> CalculateTotalAsync(IEnumerable<OrderProduct> orderProducts);

    /// <summary>
    /// Verifies if all the products have available inventory to make the order.
    /// </summary>
    /// <param name="orderProducts">The products to be verified.</param>
    /// <returns>Throws an exception in case of failure.</returns>
    Task VerifyInventoryAvailabilityAsync(IEnumerable<OrderProduct> orderProducts);
}
