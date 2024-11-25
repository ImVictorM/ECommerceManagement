using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.DeactivateProduct;

public partial class DeactivateProductCommandHandler
{
    private readonly ILogger<DeactivateProductCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating product deactivation. Product id: {Id}."
    )]
    private partial void LogInitiatingProductDeactivation(string id);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The product to be deactivated either does not exist or is already inactive."
    )]
    private partial void LogProductToBeDeactivateDoesNotExist();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The product was deactivated and saved successfully."
    )]
    private partial void LogDeactivationCompleted();
}
