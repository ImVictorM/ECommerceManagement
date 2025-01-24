using Domain.OrderAggregate;
using Domain.OrderAggregate.Events;
using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.TestUtils.Events.Orders;

/// <summary>
/// Utilities for the <see cref="OrderCanceled"/> class.
/// </summary>
public static class OrderCanceledUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="OrderCanceled"/> class.
    /// </summary>
    /// <param name="order">The canceled order.</param>
    /// <returns>A new instance of the <see cref="OrderCanceled"/> class.</returns>
    public static async Task<OrderCanceled> CreateEventAsync(
        Order? order = null
    )
    {
        return new OrderCanceled(
            order ?? await OrderUtils.CreateOrderAsync()
        );
    }
}
