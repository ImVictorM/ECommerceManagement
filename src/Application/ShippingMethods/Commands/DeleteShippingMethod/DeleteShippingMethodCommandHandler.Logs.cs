using Microsoft.Extensions.Logging;

namespace Application.ShippingMethods.Commands.DeleteShippingMethod;

internal sealed partial class DeleteShippingMethodCommandHandler
{
    private readonly ILogger<DeleteShippingMethodCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Starting removal of shipping method with id {Id}."
    )]
    private partial void LogInitiatingDeleteShippingMethod(string id);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The shipping method could not be deleted because it was not found."
    )]
    private partial void LogShippingMethodNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The shipping method was deleted successfully."
    )]
    private partial void LogShippingMethodDeletedSuccessfully();
}
