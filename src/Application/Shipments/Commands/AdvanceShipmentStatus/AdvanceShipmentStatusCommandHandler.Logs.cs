using Microsoft.Extensions.Logging;

namespace Application.Shipments.Commands.AdvanceShipmentStatus;

internal sealed partial class AdvanceShipmentStatusCommandHandler
{
    private readonly ILogger<AdvanceShipmentStatusCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating to advance the shipment status. Shipment id: {ShipmentId}"
    )]
    private partial void LogInitiatingAdvanceShipmentStatus(string shipmentId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The shipment status could not be advanced because the shipment was not found."
    )]
    private partial void LogShipmentNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "Current shipment status: {Status}."
    )]
    private partial void LogCurrentShipmentStatus(string status);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "Shipment status advanced to: {Status}."
    )]
    private partial void LogShipmentStatusAdvancedTo(string status);

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message = "The shipment status was successfully advanced."
    )]
    private partial void LogShipmentStatusAdvancedSuccessfully();
}
