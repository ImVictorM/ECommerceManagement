using Domain.CarrierAggregate.ValueObjects;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate.Enumerations;
using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.ShipmentAggregate.Errors;

using SharedKernel.Errors;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;
using SharedKernel.Models;

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
            ShippingMethodId.Create(20),
            AddressUtils.CreateAddress()
        ]
    ];

    /// <summary>
    /// Tests it is possible to create a shipment correctly.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <param name="carrierId">The carrier id.</param>
    /// <param name="shippingMethodId">The shipping method id.</param>
    /// <param name="deliveryAddress">The delivery address.</param>
    [Theory]
    [MemberData(nameof(ShipmentValidCreationParameters))]
    public void CreateShipment_WithValidParameters_CreatesWithoutThrowing(
        OrderId orderId,
        CarrierId carrierId,
        ShippingMethodId shippingMethodId,
        Address deliveryAddress
    )
    {
        var actionResult = FluentActions
            .Invoking(() => ShipmentUtils.CreateShipment(
                orderId: orderId,
                carrierId: carrierId,
                shippingMethodId: shippingMethodId,
                deliveryAddress: deliveryAddress
            ))
            .Should()
            .NotThrow();

        var shipment = actionResult.Subject;

        shipment.OrderId.Should().Be(orderId);
        shipment.CarrierId.Should().Be(carrierId);
        shipment.ShippingMethodId.Should().Be(shippingMethodId);
        shipment.DeliveryAddress.Should().Be(deliveryAddress);
        shipment.ShipmentStatus.Should().Be(ShipmentStatus.Pending);
        shipment.ShipmentTrackingEntries.Should().HaveCount(1);
        shipment.ShipmentTrackingEntries.Should().Contain(s => s.ShipmentStatus == ShipmentStatus.Pending);
    }

    /// <summary>
    /// Tests advancing shipment status moves to the next status correctly.
    /// </summary>
    [Fact]
    public void AdvanceShipmentStatus_WhenNotDelivered_MovesToNextStatus()
    {
        var shipment = ShipmentUtils.CreateShipment();

        var shipmentStatusesSequence = BaseEnumeration
            .GetAll<ShipmentStatus>()
            .Where(s => s.Id > 0)
            .OrderBy(s => s.Id)
            .ToList();

        var lastShipmentStatus = shipmentStatusesSequence.Last();

        foreach (var expectedStatus in shipmentStatusesSequence)
        {
            shipment.ShipmentStatus.Should().Be(expectedStatus);

            if (shipment.ShipmentStatus != lastShipmentStatus)
            {
                shipment.AdvanceShipmentStatus();
            }
        }

        shipment.ShipmentStatus.Should().Be(lastShipmentStatus);
    }

    /// <summary>
    /// Tests that trying to advance shipment status after it has been delivered throws an exception.
    /// </summary>
    [Fact]
    public void AdvanceShipmentStatus_WhenDelivered_ThrowsOutOfRangeException()
    {
        var shipment = ShipmentUtils.CreateShipment();

        var shipmentStatusesSequence = BaseEnumeration
            .GetAll<ShipmentStatus>()
            .Where(s => s.Id > 0)
            .OrderBy(s => s.Id)
            .ToList();

        foreach (var _ in shipmentStatusesSequence.SkipLast(1))
        {
            shipment.AdvanceShipmentStatus();
        }

        shipment.ShipmentStatus.Should().Be(ShipmentStatus.Delivered);

        FluentActions
            .Invoking(shipment.AdvanceShipmentStatus)
            .Should()
            .Throw<OutOfRangeException>();
    }

    /// <summary>
    /// Tests that a pending shipment can be canceled.
    /// </summary>
    [Fact]
    public void Cancel_WhenShipmentIsPending_UpdatesStatusToCanceled()
    {
        var shipment = ShipmentUtils.CreateShipment();

        FluentActions.Invoking(shipment.Cancel)
            .Should()
            .NotThrow();

        shipment.ShipmentStatus.Should().Be(ShipmentStatus.Canceled);
    }

    /// <summary>
    /// Tests that attempting to cancel a shipment that is not pending throws an exception.
    /// </summary>
    [Fact]
    public void Cancel_WhenShipmentIsNotPending_ThrowsShipmentCannotBeCanceledException()
    {
        var shipment = ShipmentUtils.CreateShipment();

        shipment.AdvanceShipmentStatus();

        FluentActions
            .Invoking(shipment.Cancel)
            .Should()
            .Throw<ShipmentCannotBeCanceledException>();
    }
}
