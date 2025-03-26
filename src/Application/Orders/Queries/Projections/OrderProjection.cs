using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Application.Orders.Queries.Projections;

/// <summary>
/// Represents an order projection.
/// </summary>
/// <param name="Id">The order identifier.</param>
/// <param name="OwnerId">The order owner identifier.</param>
/// <param name="Description">The order description.</param>
/// <param name="OrderStatus">The order status.</param>
/// <param name="Total">The order total.</param>
public record OrderProjection(
    OrderId Id,
    UserId OwnerId,
    string Description,
    OrderStatus OrderStatus,
    decimal Total
);
