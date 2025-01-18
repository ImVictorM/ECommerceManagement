using Application.Orders.Queries.GetOrderById;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Orders.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetOrderByIdQuery"/> class.
/// </summary>
public static class GetOrderByIdQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetOrderByIdQuery"/> class.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <returns>A new instance of the <see cref="GetOrderByIdQuery"/> class.</returns>
    public static GetOrderByIdQuery CreateQuery(
        string? orderId = null
    )
    {
        return new GetOrderByIdQuery(
            orderId ?? NumberUtils.CreateRandomLongAsString()
        );
    }
}
