using Microsoft.Extensions.Logging;

namespace Application.ShippingMethods.Commands.UpdateShippingMethod;

internal sealed partial class UpdateShippingMethodCommandHandler
{
    private readonly ILogger<UpdateShippingMethodCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message =
        "Initiating shipping method update. Shipping method identifier: '{Id}'."
    )]
    private partial void LogInitiatingShippingMethodUpdate(string id);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The shipping method could not be updated because it was not found."
    )]
    private partial void LogShippingMethodNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The shipping method has been updated. Initiating persistence."
    )]
    private partial void LogShippingMethodUpdated();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message =
        "The shipping method has been updated and saved. " +
        "The operation was completed successfully."
    )]
    private partial void LogShippingMethodChangesSaved();
}
