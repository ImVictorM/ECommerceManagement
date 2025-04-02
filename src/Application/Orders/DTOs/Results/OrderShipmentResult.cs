using Application.Orders.Queries.Projections;

using SharedKernel.ValueObjects;

namespace Application.Orders.DTOs.Results;

/// <summary>
/// Represents an order shipment result.
/// </summary>
public class OrderShipmentResult
{
    /// <summary>
    /// Gets the shipment identifier.
    /// </summary>
    public string ShipmentId { get; }
    /// <summary>
    /// Gets the shipment status.
    /// </summary>
    public string Status { get; }
    /// <summary>
    /// Gets the shipment delivery address.
    /// </summary>
    public Address DeliveryAddress { get; }
    /// <summary>
    /// Gets the shipment shipping method.
    /// </summary>
    public OrderShippingMethodResult ShippingMethod { get; }

    private OrderShipmentResult(OrderShipmentProjection projection)
    {
        ShipmentId = projection.Id.ToString();
        Status = projection.ShipmentStatus.Name;
        DeliveryAddress = projection.DeliveryAddress;
        ShippingMethod = OrderShippingMethodResult
            .FromProjection(projection.ShippingMethod);
    }

    internal static OrderShipmentResult FromProjection(
        OrderShipmentProjection projection
    )
    {
        return new OrderShipmentResult(projection);
    }
}
