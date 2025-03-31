using Application.Common.PaymentGateway.Responses;
using Application.Orders.DTOs.Results;
using Application.Orders.Queries.Projections;

using FluentAssertions;

namespace Application.UnitTests.Orders.TestUtils.Extensions;

/// <summary>
/// Extension method for the <see cref="OrderDetailedResult"/> class.
/// </summary>
public static class OrderDetailedResultExtensions
{
    /// <summary>
    /// Ensures an <see cref="OrderDetailedResult"/> matches the results
    /// from a <see cref="OrderDetailedProjection"/> projection and
    /// <see cref="PaymentResponse"/> payment response.
    /// </summary>
    /// <param name="result">The current result.</param>
    /// <param name="projection">The query projection.</param>
    /// <param name="paymentResponse">The payment response.</param>
    public static void EnsureCorrespondsTo(
        this OrderDetailedResult result,
        OrderDetailedProjection projection,
        PaymentResponse paymentResponse
    )
    {
        result.Should().NotBeNull();
        result.Id.Should().Be(projection.Id.ToString());
        result.OwnerId.Should().Be(projection.OwnerId.ToString());
        result.Description.Should().Be(projection.Description);
        result.Status.Should().Be(projection.OrderStatus.Name);
        result.Total.Should().Be(projection.Total);
        result.Products.Should().BeEquivalentTo(projection.Products);

        result.Shipment.ShipmentId.Should().Be(projection.Shipment.Id.ToString());
        result.Shipment.Status.Should().Be(projection.Shipment.ShipmentStatus.Name);
        result.Shipment.DeliveryAddress
            .Should().Be(projection.Shipment.DeliveryAddress);

        result.Shipment.ShippingMethod.Name
            .Should().Be(projection.Shipment.ShippingMethod.Name);
        result.Shipment.ShippingMethod.EstimatedDeliveryDays
            .Should().Be(projection.Shipment.ShippingMethod.EstimatedDeliveryDays);
        result.Shipment.ShippingMethod.Price
            .Should().Be(projection.Shipment.ShippingMethod.Price);

        result.Payment.PaymentId.Should().Be(projection.PaymentId.ToString());
        result.Payment.Amount.Should().Be(paymentResponse.Amount);
        result.Payment.Status.Should().Be(paymentResponse.Status.Name);
        result.Payment.PaymentMethod.Should().Be(paymentResponse.PaymentMethod.Type);
        result.Payment.Details.Should().Be(paymentResponse.Details);
    }
}
