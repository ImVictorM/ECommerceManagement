using Microsoft.Extensions.Logging;

namespace Application.ShippingMethods.Queries.GetShippingMethodById;

internal sealed partial class GetShippingMethodByIdQueryHandler
{
    private readonly ILogger<GetShippingMethodByIdQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating retrieval of shipping method with identifier '{Id}'."
    )]
    private partial void LogInitiatingShippingMethodRetrieval(string id);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message =
        "The shipping method could not be retrieved because it does not exist."
    )]
    private partial void LogShippingMethodNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message =
        "The shipping method has been retrieved. " +
        "The operation was completed successfully."
    )]
    private partial void LogShippingMethodRetrievedSuccessfully();
}
