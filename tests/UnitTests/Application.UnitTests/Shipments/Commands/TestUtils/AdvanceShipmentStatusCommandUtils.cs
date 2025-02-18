using Application.Shipments.Commands.AdvanceShipmentStatus;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Shipments.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="AdvanceShipmentStatusCommand"/> class.
/// </summary>
public static class AdvanceShipmentStatusCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="AdvanceShipmentStatusCommand"/> class.
    /// </summary>
    /// <param name="shipmentId">The shipment id.</param>
    /// <returns>A new instance of the <see cref="AdvanceShipmentStatusCommand"/> class.</returns>
    public static AdvanceShipmentStatusCommand CreateCommand(
        string? shipmentId = null
    )
    {
        return new AdvanceShipmentStatusCommand(
            shipmentId ?? NumberUtils.CreateRandomLongAsString()
        );
    }
}
