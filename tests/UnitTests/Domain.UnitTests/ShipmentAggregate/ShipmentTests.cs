using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate.Entities;
using Domain.UnitTests.TestUtils;

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
            "AccountableTest"
        ]
    ];

    /// <summary>
    /// Tests it is possible to create a shipment correctly.
    /// </summary>
    /// <param name="orderId">The order id.</param>
    /// <param name="accountable">The accountable.</param>
    [Theory]
    [MemberData(nameof(ShipmentValidCreationParameters))]
    public void CreateShipment_WithValidParameters_CreatesWithoutThrowing(
        OrderId orderId,
        string accountable
    )
    {
        var actionResult = FluentActions
            .Invoking(() => ShipmentUtils.CreateShipment(
                orderId,
                accountable
            ))
            .Should()
            .NotThrow();

        var shipment = actionResult.Subject;

        shipment.OrderId.Should().Be(orderId);
        shipment.Accountable.Should().Be(accountable);
        shipment.ShipmentStatusId.Should().Be(ShipmentStatus.Pending.Id);
        shipment.ShipmentStatusHistories.Should().HaveCount(1);
        shipment.ShipmentStatusHistories.Should().Contain(ssh => ssh.ShipmentStatusId == ShipmentStatus.Pending.Id);
    }
}
