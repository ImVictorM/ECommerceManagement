using Application.Orders.Queries.GetOrderById;
using Domain.UnitTests.TestUtils.Constants;

namespace Application.UnitTests.Orders.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetOrderByIdQuery"/> class.
/// </summary>
public static class GetOrderByIdQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetOrderByIdQuery"/> class.
    /// </summary>
    /// <param name="currentUserId">The current user id.</param>
    /// <param name="orderId">The order id.</param>
    /// <returns>A new instance of the <see cref="GetOrderByIdQuery"/> class.</returns>
    public static GetOrderByIdQuery CreateQuery(
        string? currentUserId = null,
        string? orderId = null
    )
    {
        return new GetOrderByIdQuery(
            currentUserId ?? DomainConstants.Order.OwnerId.ToString(),
            orderId ?? DomainConstants.Order.Id.ToString()
        );
    }
}
