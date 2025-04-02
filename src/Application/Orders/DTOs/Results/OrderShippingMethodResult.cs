using Application.Orders.Queries.Projections;

namespace Application.Orders.DTOs.Results;

/// <summary>
/// Represents an order shipping method result.
/// </summary>
public class OrderShippingMethodResult
{
    /// <summary>
    /// Gets the shipping method name.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Gets the shipping method price.
    /// </summary>
    public decimal Price { get; }
    /// <summary>
    /// Gets the shipping method estimated delivery days.
    /// </summary>
    public int EstimatedDeliveryDays { get; }

    private OrderShippingMethodResult(OrderShippingMethodProjection projection)
    {
        Name = projection.Name;
        Price = projection.Price;
        EstimatedDeliveryDays = projection.EstimatedDeliveryDays;
    }

    internal static OrderShippingMethodResult FromProjection(
        OrderShippingMethodProjection projection
    )
    {
        return new OrderShippingMethodResult(projection);
    }
}
