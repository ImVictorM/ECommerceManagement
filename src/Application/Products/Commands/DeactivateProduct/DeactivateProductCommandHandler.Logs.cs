using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.DeactivateProduct;

internal sealed partial class DeactivateProductCommandHandler
{
    private readonly ILogger<DeactivateProductCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating product deactivation. Product identifier: {Id}."
    )]
    private partial void LogInitiatingProductDeactivation(string id);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message =
        "The product to be deactivated either does not exist or is already inactive."
    )]
    private partial void LogProductToBeDeactivateDoesNotExist();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message =
        "The product has been deactivated and saved. " +
        "The operation completed successfully."
    )]
    private partial void LogDeactivationCompleted();
}
