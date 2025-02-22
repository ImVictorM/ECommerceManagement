using Microsoft.Extensions.Logging;

namespace Application.Orders.Commands.PlaceOrder;

public sealed partial class PlaceOrderCommandHandler
{
    private readonly ILogger<PlaceOrderCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Starting to place order."
    )]
    private partial void LogInitiatingPlaceOrder();

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Customer placing order identifier: {OwnerId}."
    )]
    private partial void LogOrderCustomerId(string ownerId);

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The order was created successfully."
    )]
    private partial void LogOrderCreated();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "The order was placed. The process was complete successfully."
    )]
    private partial void LogOrderPlacedSuccessfully();
}
