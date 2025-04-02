using Domain.OrderAggregate.ValueObjects;

using Application.Orders.Queries.Projections;

namespace Application.Orders.DTOs.Results;

/// <summary>
/// Represents a detailed order result.
/// </summary>
public class OrderDetailedResult
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
    /// <summary>
    /// Gets the order shipment.
    /// </summary>
    public OrderShipmentResult Shipment { get; }
    /// <summary>
    /// Gets the order payment.
    /// </summary>
    public OrderPaymentResult Payment { get; }
    /// <summary>
    /// Gets the order products.
    /// </summary>
    public IReadOnlyList<OrderLineItem> Products { get; }

    private OrderDetailedResult(
        OrderDetailedProjection projection,
        OrderPaymentResult payment
    )
    {
        Id = projection.Id.ToString();
        OwnerId = projection.OwnerId.ToString();
        Description = projection.Description;
        Status = projection.OrderStatus.Name;
        Total = projection.Total;
        Shipment = OrderShipmentResult
            .FromProjection(projection.Shipment);
        Products = projection.Products.ToList();
        Payment = payment;
    }

    internal static OrderDetailedResult FromProjectionWithPayment(
        OrderDetailedProjection projection,
        OrderPaymentResult payment
    )
    {
        return new OrderDetailedResult(projection, payment);
    }
}

