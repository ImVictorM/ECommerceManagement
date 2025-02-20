using Application.Orders.DTOs;

using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Application.Common.Persistence;

/// <summary>
/// Defines the contract for order persistence operations.
/// </summary>
public interface IOrderRepository : IBaseRepository<Order, OrderId>
{
    /// <summary>
    /// Retrieves an order containing details such as the order payment and shipment.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <param name="ownerId">The order owner id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The order containing details.</returns>
    Task<OrderDetailedQueryResult?> GetOrderDetailedAsync(
        OrderId orderId,
        UserId ownerId,
        CancellationToken cancellationToken = default
   );

    /// <summary>
    /// Retrieves an order containing details such as the order payment and shipment.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The order containing details.</returns>
    Task<OrderDetailedQueryResult?> GetOrderDetailedAsync(
        OrderId orderId,
        CancellationToken cancellationToken = default
   );
}
