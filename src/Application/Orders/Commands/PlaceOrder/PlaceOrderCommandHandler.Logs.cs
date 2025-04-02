using Microsoft.Extensions.Logging;

namespace Application.Orders.Commands.PlaceOrder;

internal sealed partial class PlaceOrderCommandHandler
{
    private readonly ILogger<PlaceOrderCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating order placement process."
    )]
    private partial void LogInitiatingOrderPlacement();

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Customer placing order identifier: '{OwnerId}'."
    )]
    private partial void LogOrderOwnerIdentifier(string ownerId);

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The order object has been created successfully."
    )]
    private partial void LogOrderCreated();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message =
        "The order has been placed. " +
        "The operation was completed successfully."
    )]
    private partial void LogOrderPlacedSuccessfully();
}
