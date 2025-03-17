using Application.Common.PaymentGateway;
using Application.Orders.DTOs;

using FluentAssertions;

namespace Application.UnitTests.Orders.TestUtils.Extensions;

/// <summary>
/// Extension method for the <see cref="OrderDetailedResult"/> class.
/// </summary>
public static class OrderDetailedResultExtensions
{
    /// <summary>
    /// Ensures an <see cref="OrderDetailedResult"/> matches the results
    /// from a <see cref="OrderDetailedQueryResult"/> query and
    /// <see cref="PaymentResponse"/> payment response.
    /// </summary>
    /// <param name="result">The current result.</param>
    /// <param name="query">The query.</param>
    /// <param name="paymentResponse">The payment response.</param>
    public static void EnsureCorrespondsTo(
        this OrderDetailedResult result,
        OrderDetailedQueryResult query,
        PaymentResponse paymentResponse
    )
    {
        result.Should().NotBeNull();
        result.Order.Should().BeEquivalentTo(query.Order);

        result.Shipment.ShipmentId.Should().Be(query.OrderShipment.ShipmentId);
        result.Shipment.Status.Should().Be(query.OrderShipment.Status);
        result.Shipment.DeliveryAddress
            .Should()
            .Be(query.OrderShipment.DeliveryAddress);
        result.Shipment.ShippingMethod.Name
            .Should()
            .Be(query.OrderShipment.ShippingMethod.Name);
        result.Shipment.ShippingMethod.EstimatedDeliveryDays
            .Should()
            .Be(query.OrderShipment.ShippingMethod.EstimatedDeliveryDays);
        result.Shipment.ShippingMethod.Price
            .Should()
            .Be(query.OrderShipment.ShippingMethod.Price);

        result.Payment.PaymentId.Should().Be(query.PaymentId);
        result.Payment.Amount.Should().Be(paymentResponse.Amount);
        result.Payment.Status.Should().Be(paymentResponse.Status.Name);
        result.Payment.PaymentMethod.Should().Be(paymentResponse.PaymentMethod);
        result.Payment.Details.Should().Be(paymentResponse.Details);
    }
}
