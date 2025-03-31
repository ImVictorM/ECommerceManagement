using Application.Orders.Queries.Projections;

using Domain.OrderAggregate;

namespace Application.UnitTests.Orders.TestUtils.Projections;

/// <summary>
/// Utilities for the <see cref="OrderProjection"/> class.
/// </summary>
public static class OrderProjectionUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="OrderProjection"/> class.
    /// </summary>
    /// <param name="order">The order representing the projection.</param>
    /// <returns>
    /// A new instance of the <see cref="OrderProjection"/> class.
    /// </returns>
    public static OrderProjection CreateProjection(Order order)
    {
        return new OrderProjection(
            order.Id,
            order.OwnerId,
            order.Description,
            order.OrderStatus,
            order.Total
        );
    }

    /// <summary>
    /// Creates  a list of <see cref="OrderProjection"/> based
    /// on the given orders.
    /// </summary>
    /// <param name="orders">The orders representing the projections.</param>
    /// <returns>A list of <see cref="OrderProjection"/>.</returns>
    public static IReadOnlyList<OrderProjection> CreateProjections(
        IEnumerable<Order> orders
    )
    {
        return orders.Select(CreateProjection).ToList();
    }
}
