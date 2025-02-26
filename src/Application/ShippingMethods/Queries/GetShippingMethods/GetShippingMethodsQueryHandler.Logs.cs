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
        Message = "Shipping methods retrieved successfully. Quantity of shipping methods retrieved: {Count}."
    )]
    private partial void LogShippingMethodsQuantityRetrieved(int count);
}
