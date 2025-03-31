using Application.Orders.DTOs.Filters;
using Application.Orders.Queries.GetOrders;

namespace Application.UnitTests.Orders.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetOrdersQuery"/> class.
/// </summary>
public static class GetOrdersQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetOrdersQuery"/> class.
    /// </summary>
    /// <param name="filters">The order filters.</param>
    /// <returns>
    /// A new instance of the <see cref="GetOrdersQuery"/> class.
    /// </returns>
    public static GetOrdersQuery CreateQuery(OrderFilters? filters = null)
    {
        return new GetOrdersQuery(filters ?? new OrderFilters());
    }
}
