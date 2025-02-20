using Microsoft.Extensions.Logging;

namespace Application.Orders.Queries.GetOrders;

public partial class GetOrdersQueryHandler
{
    private readonly ILogger<GetOrdersQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating orders retrieval. Filter status condition: {FilterStatusCondition}."
    )]
    private partial void LogInitiatingOrdersRetrieval(string? filterStatusCondition);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Orders retrieved successfully. Quantity of orders found: {Count}."
    )]
    private partial void LogOrdersRetrievedSuccessfully(int count);
}
