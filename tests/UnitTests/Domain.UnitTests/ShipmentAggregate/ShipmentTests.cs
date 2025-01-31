using Domain.CarrierAggregate.ValueObjects;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate.Enumerations;
using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using SharedKernel.Errors;

using FluentAssertions;

namespace Domain.UnitTests.ShipmentAggregate;

/// <summary>
/// Unit tests for the <see cref="Domain.ShipmentAggregate.Shipment"/> class.
/// </summary>
public class ShipmentTests
{
    /// <summary>
    /// List of valid shipment creation parameters.
    /// </summary>
    public static readonly IEnumerable<object[]> ShipmentValidCreationParameters =
    [
        [
            OrderId.Create(19),
            CarrierId.Create(10),
            ShippingMethodId.Create(20)
        ]
    ];

    /// <summary>
    /// Tests it is possible to create a shipment correctly.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <param name="carrierId">The carrier id.</param>
    /// <param name="shippingMethodId">The shipping method id.</param>
    [Theory]
    [MemberData(nameof(ShipmentValidCreationParameters))]
    public void CreateShipment_WithValidParameters_CreatesWithoutThrowing(
        OrderId orderId,
        CarrierId carrierId,
        ShippingMethodId shippingMethodId
    )
    {
        var actionResult = FluentActions
            .Invoking(() => ShipmentUtils.CreateShipment(
                orderId,
                carrierId,
                shippingMethodId
            ))
            .Should()
            .NotThrow();

        var shipment = actionResult.Subject;

        shipment.OrderId.Should().Be(orderId);
        shipment.CarrierId.Should().Be(carrierId);
        shipment.ShippingMethodId.Should().Be(shippingMethodId);
        shipment.ShipmentStatus.Should().Be(ShipmentStatus.Pending);
        shipment.ShipmentTrackingEntries.Should().HaveCount(1);
        shipment.ShipmentTrackingEntries.Should().Contain(s => s.ShipmentStatus == ShipmentStatus.Pending);
    }

    /// <summary>
    /// Verifies if the <see cref="Domain.ShipmentAggregate.Shipment.AdvanceShipmentStatus"/> works correctly. 
    /// </summary>
    [Fact]
    public void AdvanceShipmentStatus_WhenCalled_MovesToNextStatusOrThrowsWhenOutOfRange()
    {
        var orderedShipmentStatuses = ShipmentStatus.List().OrderBy(s => s.Id).ToList();
        var lastShipmentStatus = orderedShipmentStatuses.Last();
        var shipment = ShipmentUtils.CreateShipment();

        foreach (var shipmentStatus in orderedShipmentStatuses)
        {
            shipment.ShipmentStatus.Should().Be(shipmentStatus);

            if (lastShipmentStatus != shipment.ShipmentStatus)
            {
                shipment.AdvanceShipmentStatus();
            }
        }

        FluentActions
            .Invoking(shipment.AdvanceShipmentStatus)
            .Should()
            .Throw<OutOfRangeException>();
    }
}
