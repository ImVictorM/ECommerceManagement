using Application.Orders.Queries.GetCustomerOrderById;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Orders.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetCustomerOrderByIdQuery"/> class.
/// </summary>
public static class GetCustomerOrderByIdQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetCustomerOrderByIdQuery"/> class.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="orderId">The order identifier.</param>
    /// <returns>
    /// A new instance of the <see cref="GetCustomerOrderByIdQuery"/> class.
    /// </returns>
    public static GetCustomerOrderByIdQuery CreateQuery(
        string? userId = null,
        string? orderId = null
    )
    {
        return new GetCustomerOrderByIdQuery(
            userId ?? NumberUtils.CreateRandomLongAsString(),
            orderId ?? NumberUtils.CreateRandomLongAsString()
        );
    }
}
