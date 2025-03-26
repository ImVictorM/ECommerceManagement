using Microsoft.Extensions.Logging;

namespace Application.ShippingMethods.Queries.GetShippingMethods;

internal sealed partial class GetShippingMethodsQueryHandler
{
    private readonly ILogger<GetShippingMethodsQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating shipping methods retrieval."
    )]
    private partial void LogInitiatingShippingMethodsRetrieval();

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message =
        "{Count} shipping methods has been retrieved. " +
        "The operation was completed successfully."
    )]
    private partial void LogShippingMethodsQuantityRetrieved(int count);
}
