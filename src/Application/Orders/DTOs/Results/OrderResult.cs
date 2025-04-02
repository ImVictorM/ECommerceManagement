using Domain.OrderAggregate;

using Application.Orders.Queries.Projections;

namespace Application.Orders.DTOs.Results;

/// <summary>
/// Represents an order result.
/// </summary>
public class OrderResult
{
    /// <summary>
    /// Gets the order identifier.
    /// </summary>
    public string Id { get; }
    /// <summary>
    /// Gets the order owner identifier.
    /// </summary>
    public string OwnerId { get; }
    /// <summary>
    /// Gets the order description.
    /// </summary>
    public string Description { get; }
    /// <summary>
    /// Gets the order status.
    /// </summary>
    public string Status { get; }
    /// <summary>
    /// Gets the order total.
    /// </summary>
    public decimal Total { get; }

    private OrderResult(Order order)
    {
        Id = order.Id.ToString();
        OwnerId = order.OwnerId.ToString();
        Description = order.Description;
        Status = order.OrderStatus.Name;
        Total = order.Total;
    }

    private OrderResult(OrderProjection projection)
    {
        Id = projection.Id.ToString();
        OwnerId = projection.OwnerId.ToString();
        Description = projection.Description;
        Status = projection.OrderStatus.Name;
        Total = projection.Total;
    }

    internal static OrderResult FromOrder(Order order)
    {
        return new OrderResult(order);
    }

    internal static OrderResult FromProjection(OrderProjection projection)
    {
        return new OrderResult(projection);
    }
};
