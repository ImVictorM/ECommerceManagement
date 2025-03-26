using Application.Orders.DTOs.Filters;
using Application.Orders.Queries.Projections;

using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Application.Common.Persistence.Repositories;

/// <summary>
/// Defines the contract for order persistence operations.
/// </summary>
public interface IOrderRepository : IBaseRepository<Order, OrderId>
{
    /// <summary>
    /// Retrieves an order containing details such as the order payment and shipment.
    /// </summary>
    /// <param name="orderId">The order identifier.</param>
    /// <param name="ownerId">The order owner identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of <see cref="OrderDetailedProjection"/>.</returns>
    Task<OrderDetailedProjection?> GetCustomerOrderDetailedAsync(
        OrderId orderId,
        UserId ownerId,
        CancellationToken cancellationToken = default
   );

    /// <summary>
    /// Retrieves customer orders with filtering support.
    /// </summary>
    /// <param name="ownerId">The order owner identifier.</param>
    /// <param name="filters">The filtering criteria.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of <see cref="OrderProjection"/>.</returns>
    Task<IReadOnlyList<OrderProjection>> GetCustomerOrdersAsync(
        UserId ownerId,
        OrderFilters? filters = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves an order containing details such as the order payment and shipment.
    /// </summary>
    /// <param name="orderId">The order identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of <see cref="OrderDetailedProjection"/>.</returns>
    Task<OrderDetailedProjection?> GetOrderDetailedAsync(
        OrderId orderId,
        CancellationToken cancellationToken = default
   );

    /// <summary>
    /// Retrieves orders with filtering support.
    /// </summary>
    /// <param name="filters">The filtering criteria.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of <see cref="OrderProjection"/>.</returns>
    Task<IReadOnlyList<OrderProjection>> GetOrdersAsync(
        OrderFilters? filters = default,
        CancellationToken cancellationToken = default
    );
}
