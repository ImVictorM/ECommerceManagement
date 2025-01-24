using Domain.OrderAggregate.Events;
using Domain.OrderAggregate;
using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.TestUtils.Events.Orders;

/// <summary>
/// Utilities for the <see cref="OrderPaid"/> class.
/// </summary>
public static class OrderPaidUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="OrderPaid"/> class.
    /// </summary>
    /// <param name="order">The paid order.</param>
    /// <returns>A new instance of the <see cref="OrderPaid"/> class.</returns>
    public static async Task<OrderPaid> CreateEventAsync(
        Order? order = null
    )
    {
        return new OrderPaid(
            order ?? await OrderUtils.CreateOrderAsync()
        );
    }
}
