using Application.Orders.DTOs.Filters;
using Application.Orders.Queries.GetCustomerOrders;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Orders.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetCustomerOrdersQuery"/> class.
/// </summary>
public static class GetCustomerOrdersQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetCustomerOrdersQuery"/> class.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="filters">The filters.</param>
    /// <returns>
    /// A new instance of the <see cref="GetCustomerOrdersQuery"/> class.
    /// </returns>
    public static GetCustomerOrdersQuery CreateQuery(
        string? userId = null,
        OrderFilters? filters = null
    )
    {
        return new GetCustomerOrdersQuery(
            userId ?? NumberUtils.CreateRandomLongAsString(),
            filters ?? new OrderFilters()
        );
    }
}
